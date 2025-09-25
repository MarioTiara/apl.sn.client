using SN.Core.Domain.Companies;
using SN.Core.Domain.ValueObjects;

namespace SN.Applications.Documents.Dtos;

public class CoreDocumentDto
{
    public string? FileExtension { get; private set; }
    public string? FileName { get; private set; }
    public string? DocumentIdentifier { get; private set; }
    public string? TransactionCode { get; private set; }
    public string SenderIdentifier { get; private set; } 
    public string ReceiverIdentifier { get; private set; } 
    public Company Owner { get; private set; }

    public List<AggregationNode> Aggregations { get; private set; }

    public CoreDocumentDto(
        string senderIdentifier,
        string receiverIdentifier,
        Company owner,
        string? fileName = null,
        string? fileExtension = null,
        string? documentIdentifier = null,
        string? transactionCode = null,
        List<AggregationNode>? aggregations = null)
    {
        SenderIdentifier = senderIdentifier ?? throw new ArgumentNullException(nameof(senderIdentifier));
        ReceiverIdentifier = receiverIdentifier ?? throw new ArgumentNullException(nameof(receiverIdentifier));
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));

        FileName = fileName;
        FileExtension = fileExtension;
        DocumentIdentifier = documentIdentifier;
        TransactionCode = transactionCode;
        Aggregations = aggregations ?? new List<AggregationNode>();
    }
}
