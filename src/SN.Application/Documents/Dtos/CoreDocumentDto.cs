using SN.Core.Domain.Companies;
using SN.Core.Domain.ValueObjects;

namespace SN.Applications.Documents.Dtos;

public class CoreDocumentDto
{
    public string? DocumentType { get; private set; }
    public string? FilePath { get; private set; }
    public string? DocumentIdentifier { get; private set; }
    public string DeliveryNumber { get; private set; }
    public string? SenderIdentifier { get; private set; }
    public string? ReceiverIdentifier { get; private set; }
    public DateTime? DoucmentCreationTime { get; private set; }
    public Company Producer { get; private set; }
    public List<AggregationNode> Aggregations { get; private set; }

    public CoreDocumentDto(
     string? documentType,
     string? filePath,
     string? documentIdentifier,
     string deliveryNumber,
     string? senderIdentifier,
     string? receiverIdentifier,
     DateTime? documentCreationTime,
     Company producer,
     List<AggregationNode> aggregations)
    {
        DocumentType = documentType;
        FilePath = filePath;
        DocumentIdentifier = documentIdentifier;
        DeliveryNumber = deliveryNumber;
        SenderIdentifier = senderIdentifier;
        ReceiverIdentifier = receiverIdentifier;
        DoucmentCreationTime = documentCreationTime;
        Producer = producer;
        Aggregations = aggregations ?? new List<AggregationNode>();
    }
}
