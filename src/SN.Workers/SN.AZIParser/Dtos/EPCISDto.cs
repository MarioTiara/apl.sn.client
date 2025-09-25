
using System.Xml.Serialization;
using SN.Infrastructure.EPCIS;

namespace EPCIS.DTO
{
    [XmlRoot("EPCISDocument", Namespace = "urn:epcglobal:epcis:xsd:1")]
    public class EPCISDocument
    {
        [XmlAttribute("creationDate")]
        public string? CreationDate { get; set; }

        [XmlAttribute("schemaVersion")]
        public string? SchemaVersion { get; set; }

        [XmlElement("EPCISHeader",Namespace = "")]
        public EPCISHeader? EPCISHeader { get; set; }

        [XmlElement("EPCISBody",Namespace = "")]
        public EPCISBody? EPCISBody { get; set; }
    }

    public class EPCISHeader
    {
        [XmlElement("StandardBusinessDocumentHeader", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public StandardBusinessDocumentHeader? SBDH { get; set; }

        [XmlElement("SAPHeaderExtension", Namespace = "")]
        public SAPHeaderExtension? SAPHeaderExtension { get; set; }
    }

    public class StandardBusinessDocumentHeader
    {
        [XmlElement("HeaderVersion", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public string? HeaderVersion { get; set; }

        [XmlElement("Sender", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public SBDHParty? Sender { get; set; }

        [XmlElement("Receiver", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public SBDHParty? Receiver { get; set; }

        [XmlElement("DocumentIdentification", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public DocumentIdentification? DocumentIdentification { get; set; }
    }

    public class SBDHParty
    {
        [XmlElement("Identifier", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public SBDHIdentifier? Identifier { get; set; }
    }

    public class SBDHIdentifier
    {
        [XmlAttribute("Authority")]
        public string? Authority { get; set; }

        [XmlText]
        public string? Value { get; set; }
    }

    public class DocumentIdentification
    {
        [XmlElement("Standard", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public string? Standard { get; set; }

        [XmlElement("TypeVersion", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public string? TypeVersion { get; set; }

        [XmlElement("InstanceIdentifier", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public string? InstanceIdentifier { get; set; }

        [XmlElement("Type", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public string? Type { get; set; }

        [XmlElement("CreationDateAndTime", Namespace = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader")]
        public string? CreationDateAndTime { get; set; }
    }

    public class SAPHeaderExtension
    {
        [XmlElement("SAPQueueMessageSender", Namespace = "")]
        public string? SAPQueueMessageSender { get; set; }
    }

    public class EPCISBody
    {
        [XmlElement("EventList", Namespace = "")]
        public EventList? EventList { get; set; }
    }

    public class EventList
    {
        [XmlElement("ObjectEvent", typeof(ObjectEvent), Namespace = "")]
        [XmlElement("AggregationEvent", typeof(AggregationEvent), Namespace = "")]
        public List<BaseEvent>? Events { get; set; }
    }

    public abstract class BaseEvent
    {
        [XmlElement("eventTime", Namespace = "")]
        public string? EventTime { get; set; }

        [XmlElement("eventTimeZoneOffset", Namespace = "")]
        public string? EventTimeZoneOffset { get; set; }

        [XmlElement("action", Namespace = "")]
        public string? Action { get; set; }

        [XmlElement("bizStep", Namespace = "")]
        public string? BizStep { get; set; }

        [XmlElement("disposition", Namespace = "")]
        public string? Disposition { get; set; }

        [XmlElement("readPoint", Namespace = "")]
        public ReadPoint? ReadPoint { get; set; }

        [XmlElement("bizLocation", Namespace = "")]
        public BizLocation? BizLocation { get; set; }
    }

    public class ObjectEvent : BaseEvent
    {
        [XmlArray("epcList", Namespace = "")]
        [XmlArrayItem("epc", Namespace = "")]
        public List<string>? EpcList { get; set; }

        [XmlElement("SAPExtension", Namespace = "")]
        public SAPExtension? SAPExtension { get; set; }

        [XmlElement("bizTransactionList", Namespace = "")]
        public BizTransactionList? BizTransactionList { get; set; }

        [XmlElement("extension", Namespace = "")]
        public Extension? Extension { get; set; }
    }

    public class AggregationEvent : BaseEvent
    {
        [XmlElement("parentID", Namespace = "")]
        public string? ParentID { get; set; }

        [XmlArray("childEPCs", Namespace = "")]
        [XmlArrayItem("epc", Namespace = "")]
        public List<string>? ChildEPCs { get; set; }
    }

    public class ReadPoint
    {
        [XmlElement("id", Namespace = "")]
        public string? Id { get; set; }
    }

    public class BizLocation
    {
        [XmlElement("id", Namespace = "")]
        public string? Id { get; set; }
    }

    public class SAPExtension
    {
        [XmlElement("objAttributes", Namespace = "")]
        public ObjAttributes? ObjAttributes { get; set; }
    }

    public class ObjAttributes:EPCISAttribute
    {
        [XmlElement("LOTNO", Namespace = "")]
        public override string? LOTNO { get; set; }

        [XmlElement("DATEX", Namespace = "")]
        public override string? DATEX { get; set; }

        [XmlElement("DATMF", Namespace = "")]
        public override string? DATMF { get; set; }

        [XmlElement("PACKAGINGLEVEL", Namespace = "")]
        public string? PACKAGINGLEVEL { get; set; }
    }

    public class BizTransactionList
    {
        [XmlElement("bizTransaction", Namespace = "")]
        public List<BizTransaction>? BizTransactions { get; set; }
    }

    public class BizTransaction
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlText]
        public string? Value { get; set; }
    }

    public class Extension
    {
        [XmlElement("sourceList", Namespace = "")]
        public SourceList? SourceList { get; set; }

        [XmlElement("destinationList", Namespace = "")]
        public DestinationList? DestinationList { get; set; }
    }

    public class SourceList
    {
        [XmlElement("source", Namespace = "")]
        public List<Source>? Sources { get; set; }
    }

    public class DestinationList
    {
        [XmlElement("destination", Namespace = "")]
        public List<Destination>? Destinations { get; set; }
    }

    public class Source
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlText]
        public string? Value { get; set; }
    }

    public class Destination
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlText]
        public string? Value { get; set; }
    }
}
