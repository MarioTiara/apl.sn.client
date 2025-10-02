using System.Xml.Serialization;
using EPCIS.DTO;
using SN.Applications.Documents;
using SN.Applications.Documents.Dtos;
using SN.AZIParser.Commons;
using SN.Core.Domain.Companies;
using SN.Core.Domain.Documents;
using SN.Infrastructure.EPCIS;

namespace SN.AZIParser;

public class AZIEPCISParser
{
    public SNDocument ParseToSNDocument(string xmlPath, Company company)
    {

        var doc = this.Serialize(xmlPath);
        if (doc == null)
        {
            throw new Exception("Failed to deserialize EPCIS document.");
        }
        // Extract relevant information from the EPCIS document 
        // Document level attributes
        var filePath = $"SN.AZIParse\\{xmlPath}";
        var fileName = Path.GetFileName(xmlPath);
        var fileExtension = Path.GetExtension(xmlPath);
        var creationDate = DateTime.TryParse(doc.CreationDate, out var parsedDate) ? parsedDate : (DateTime?)null;
        var schemaVersion = doc.SchemaVersion;

        var epcAttribute = this.BuildEpcAttributesMap(doc);
        // From SBDH (StandardBusinessDocumentHeader)
        var senderId = doc.EPCISHeader?.SBDH?.Sender?.Identifier?.Value;
        var senderAuthority = doc.EPCISHeader?.SBDH?.Sender?.Identifier?.Authority;
        var receiverId = doc.EPCISHeader?.SBDH?.Receiver?.Identifier?.Value;

        var docId = doc.EPCISHeader?.SBDH?.DocumentIdentification?.InstanceIdentifier;
        var docType = $"{doc.EPCISHeader?.SBDH?.DocumentIdentification?.Standard}:{doc.EPCISHeader?.SBDH?.DocumentIdentification?.TypeVersion}";
        var docCreationTime = doc.EPCISHeader?.SBDH?.DocumentIdentification?.CreationDateAndTime;

        //Aggregation events
        var epcisAggregationEventList = doc?.EPCISBody?.EventList?.Events?
                        .Where(e => e is AggregationEvent)
                        .Cast<AggregationEvent>().ToList(); 
                        
        if (epcisAggregationEventList == null || epcisAggregationEventList.Count <= 0)
        {
            throw new Exception("no epcis event found");
        }
        var eventsList = epcisAggregationEventList
            .Select(epcisEvent => (IAggregationEvent)new AggregationEventV1Adapter(epcisEvent))
            .ToList();

        var bizTransaction = doc?.EPCISBody?.EventList?.Events?
                        .Where(e => e is ObjectEvent)
                        .Cast<ObjectEvent>()
                        .Where(p => p.BizTransactionList != null && p.BizTransactionList.BizTransactions != null)
                        .Select(p => p.BizTransactionList?.BizTransactions)
                        .FirstOrDefault()?
                        .Where(p => p != null && p.Type != null && p.Type.Contains("desadv"))
                        .FirstOrDefault();
        
                    

        if (bizTransaction is null || bizTransaction.Value is null)
        {
            throw new Exception("no bizTransaction found");
        }
        var deliverNumber = bizTransaction.Value.Split(":").LastOrDefault();
        if (string.IsNullOrEmpty(deliverNumber))
        {
            throw new Exception("no delivery number found");
        }

        var aggregationsList = new EPCISAgregationBuilder(eventsList, epcAttribute).Build();
        var coreDocument = new CoreDocumentDto(
            docType ?? "EPCIS",
            filePath,
            docId,
            deliverNumber,
            senderId,
            receiverId,
            creationDate,
            company,
            aggregationsList
        );

        var snDocumentBuilder = new BarcodeDocumentBuilder(coreDocument, new AZI2DBarcodeFactory());
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