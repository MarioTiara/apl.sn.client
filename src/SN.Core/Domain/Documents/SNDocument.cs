
using SN.Core.Domain.Barcodes;
using SN.Core.Domain.Common;
using SN.Core.Domain.Companies;
using SN.Core.Domain.Epcis;

namespace SN.Core.Domain.Documents;

public class SNDocument : BaseEntity, IAggregateRoot
{
    private List<EpcisNode> _epcisNodes;
    public string? FileExtension { get; private set; }
    public string? FileName { get; private set; }
    public string? DocumentIdentifier { get; private set; }
    public string? TransactionCode { get; private set; }
    public string SenderIdentifier { get; private set; }
    public string ReceiverIdentifier { get; private set; }
    public Company Producer { get; private set; }
    public IEnumerable<EpcisNode> EpcisNodes => _epcisNodes.AsReadOnly();

    private List<PrimaryBarcode> _primaries;
    public IEnumerable<PrimaryBarcode> PrimaryBarcodes => _primaries.AsReadOnly();

    private List<SecondaryBarcode> _secondaries;
    public IEnumerable<SecondaryBarcode> SecondaryBarcodes => _secondaries.AsReadOnly();
    private List<TertiaryBarcode> _tertiaries;
    public IEnumerable<TertiaryBarcode> TertiaryBarcodes => _tertiaries.AsReadOnly();
    public SNDocument() { }

    public SNDocument(string senderIdentifier, string receiverIdentifier, Company producer)
    {

        SenderIdentifier = senderIdentifier ?? throw new ArgumentNullException(nameof(senderIdentifier));
        ReceiverIdentifier = receiverIdentifier ?? throw new ArgumentNullException(nameof(receiverIdentifier));
        Producer = producer;
        CreatedAt = DateTime.UtcNow;
        _epcisNodes = new List<EpcisNode>();
        _primaries = new List<PrimaryBarcode>();
        _secondaries = new List<SecondaryBarcode>();
        _tertiaries = new List<TertiaryBarcode>();
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

    public void AddEpcis(EpcisNode epcis)
        => _epcisNodes.Add(epcis);

    public void SetFileInfo(string? fileName, string? fileExtension, string? documentIdentifier, string? transactionCode)
    {
        FileName = fileName;
        FileExtension = fileExtension;
        DocumentIdentifier = documentIdentifier;
        TransactionCode = transactionCode;
    }


}