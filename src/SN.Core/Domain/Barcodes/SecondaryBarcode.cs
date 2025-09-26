using System.Collections.Immutable;
using SN.Core.Domain.Documents;
using SN.Core.Domain.ValueObjects;

namespace SN.Core.Domain.Barcodes;

public class SecondaryBarcode : BarcodeAgregation
{
    private List<PrimaryBarcode> _primaries;

    public Guid? ParentId { get; private set; }
    public TertiaryBarcode? Parent { get; private set; }

    // Navigation property ke detail
    public Guid DetailId { get; private set; }
    public Barcode Detail { get; private set; }
    public IEnumerable<PrimaryBarcode> Primaries => _primaries.ToImmutableList().AsReadOnly();
    public SecondaryBarcode(){}
    public SecondaryBarcode(IBPOM2DBarcode barcode, TertiaryBarcode? parent, SNDocument document)
    {
           DocumentId = document.Id;
        BPOM2DBarCode = barcode.Get2DBarcode();
        RegistrationStatus = RegistrationStatus.Pending;
        Detail = new Barcode(barcode);
        DetailId = Detail.Id;
        Document = document;
        Parent = parent;
        ParentId = parent?.Id;
        _primaries = new List<PrimaryBarcode>();
    }

    public void AddPrimary(PrimaryBarcode primary)
    => _primaries.Add(primary);
}