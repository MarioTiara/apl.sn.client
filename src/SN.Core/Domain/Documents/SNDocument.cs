
using SN.Core.Domain.Barcodes;
using SN.Core.Domain.Common;
using SN.Core.Domain.Companies;
using SN.Core.Domain.SerialNode;

namespace SN.Core.Domain.Documents;

public class SNDocument : BaseEntity, IAggregateRoot
{
    public string? DocumentType { get; private set; }
    public string? FilePath { get; private set; }
    public string? DocumentIdentifier { get; private set; }
    public string DeliveryNumber { get; private set; }
    public string? SenderIdentifier { get; private set; }
    public string? ReceiverIdentifier { get; private set; }
    public DateTime? DocumentCreationTime { get; private set; }
    public Company Producer { get; private set; }

    private List<SerializedNode> _serializedNodes;
    public IEnumerable<SerializedNode> EpcisNodes => _serializedNodes.AsReadOnly();
    private List<PrimaryBarcode> _primaries;
    public IEnumerable<PrimaryBarcode> PrimaryBarcodes => _primaries.AsReadOnly();
    private List<SecondaryBarcode> _secondaries;
    public IEnumerable<SecondaryBarcode> SecondaryBarcodes => _secondaries.AsReadOnly();
    private List<TertiaryBarcode> _tertiaries;
    public IEnumerable<TertiaryBarcode> TertiaryBarcodes => _tertiaries.AsReadOnly();
    protected SNDocument() { }

    public SNDocument(string? senderIdentifier, string? receiverIdentifier, string deliverNumber, Company producer)
    {
        SenderIdentifier = senderIdentifier ?? throw new ArgumentNullException(nameof(senderIdentifier));
        ReceiverIdentifier = receiverIdentifier ?? throw new ArgumentNullException(nameof(receiverIdentifier));
        Producer = producer;
        DeliveryNumber = deliverNumber;
        CreatedAt = DateTime.UtcNow;
        _serializedNodes = new List<SerializedNode>();
        _primaries = new List<PrimaryBarcode>();
        _secondaries = new List<SecondaryBarcode>();
        _tertiaries = new List<TertiaryBarcode>();
    }

    public void AddSerializedNode(SerializedNode epcisNode)
    {
        _serializedNodes.Add(epcisNode);
    }
    public void AddBarcode(BarcodeAgregation barcode)
    {
        if (barcode is PrimaryBarcode)
        {
            _primaries.Add((PrimaryBarcode)barcode);
        }
        else if (barcode is SecondaryBarcode)
        {
            _secondaries.Add((SecondaryBarcode)barcode);
        }
        else if (barcode is TertiaryBarcode)
        {
            _tertiaries.Add((TertiaryBarcode)barcode);
        }
    }

    public void SetFileInfo(string? filePath, string? documentType, string? documentIdentifier, DateTime? documentCreationTime)
    {
        FilePath = filePath;
        DocumentType = documentType;
        DocumentIdentifier = documentIdentifier;
        DocumentCreationTime = documentCreationTime;
    }
    
}