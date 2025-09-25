using System.Xml.Serialization;
using EPCIS.DTO;
using SN.Applications.Documents;
using SN.Applications.Documents.Dtos;
using SN.Core.Domain.Companies;
using SN.Core.Domain.Documents;
using SN.Infrastructure.EPCIS;

namespace SN.AZIParser;

public class AZIEPCISParser
{
    public SNDocument ParseToSNDocument(string xmlPath, Company company)
    {
        var doc = this.Serialize(xmlPath);

        // Document level attributes
        var creationDate = doc.CreationDate;
        var schemaVersion = doc.SchemaVersion;

        var epcAttribute = this.BuildEpcAttributesMap(doc);
        // From SBDH (StandardBusinessDocumentHeader)
        var senderId = doc.EPCISHeader?.SBDH?.Sender?.Identifier?.Value;
        var senderAuthority = doc.EPCISHeader?.SBDH?.Sender?.Identifier?.Authority;

        var receiverId = doc.EPCISHeader?.SBDH?.Receiver?.Identifier?.Value;

        var docId = doc.EPCISHeader?.SBDH?.DocumentIdentification?.InstanceIdentifier;
        var docType = doc.EPCISHeader?.SBDH?.DocumentIdentification?.Type;
        var docCreationTime = doc.EPCISHeader?.SBDH?.DocumentIdentification?.CreationDateAndTime;

        // SAP extension
        var sapSender = doc.EPCISHeader?.SAPHeaderExtension?.SAPQueueMessageSender;
        // var  batch= doc.EPCISBody?.EventList.Events.

        //Aggregation events
        var epcisAggregationEventList = doc?.EPCISBody?.EventList?.Events?
                        .Where(e => e is AggregationEvent)
                        .Cast<AggregationEvent>().ToList(); ;

        if (epcisAggregationEventList == null || epcisAggregationEventList.Count <= 0)
        {
            throw new Exception("no epcis event found");
        }
        var eventsList = new List<IAggregationEvent>();
        foreach (var epcisEvent in epcisAggregationEventList)
        {
            eventsList.Add(new AggregationEventV1Adapter(epcisEvent));
        }

        var aggregationsList = new EPCISAgregationBuilder(eventsList, epcAttribute).Build();
        var coreDocument = new CoreDocumentDto(senderId,
        receiverId, company,
        "Dummy",
        docType,
        docId,
        "dummy",
        aggregationsList
        );

        var snDocumentBuilder = new BarcodeDocumentBuilder(coreDocument);
        var snDocument = snDocumentBuilder.GetResult();

        return snDocument;
        // var barcodeDocumentBuilder= new BarcodeDocumentBuilder()

    }


    private EPCISDocument? Serialize(string documentPath)
    {
        var serializer = new XmlSerializer(typeof(EPCISDocument));
        using (StreamReader reader = new StreamReader(documentPath))
        {
            try
            {
                var epicDocuments = (EPCISDocument?)serializer.Deserialize(reader);
                return epicDocuments;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                // _logger.LogError(error, $"{MethodBase.GetCurrentMethod()} - {error.Message}");
            }
            return null;
        }

    }

    // Build mapping EPC -> ObjAttributes
    private Dictionary<string, EPCISAttribute> BuildEpcAttributesMap(EPCISDocument doc)
    {
        var map = new Dictionary<string, EPCISAttribute>();

        var objectEvents = doc.EPCISBody?.EventList?.Events?.OfType<ObjectEvent>()
                           ?? Enumerable.Empty<ObjectEvent>();

        foreach (var objEvent in objectEvents)
        {
            if (objEvent.SAPExtension?.ObjAttributes != null)
            {
                foreach (var epc in objEvent.EpcList ?? Enumerable.Empty<string>())
                {
                    if (!map.ContainsKey(epc))
                    {
                        map[epc] = objEvent.SAPExtension.ObjAttributes;
                    }
                }
            }
        }

        return map;
    }

}