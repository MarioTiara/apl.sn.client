using System.Collections.Immutable;
using SN.Core.Domain.ValueObjects;

namespace SN.Core.Domain.Barcodes;

public class SecondaryBarcode : BarcodeAgregation
{
    private List<PrimaryBarcode> _primaries;

    public Guid? ParentId { get; private set; }
    public TertiaryBarcode? Parent { get; private set; }

    // Navigation property ke detail
    public Barcode Detail { get; private set; }
    public IEnumerable<PrimaryBarcode> Primaries => _primaries.ToImmutableList().AsReadOnly();
    public SecondaryBarcode(){}
    public SecondaryBarcode(BPOM2DBarcode barcode, TertiaryBarcode? parent, Guid documentId)
    {
           DocumentId = documentId;
        BPOM2DBarCode = barcode.ToString();
        RegistrationStatus = RegistrationStatus.Pending;
        Detail = new Barcode(barcode);
        Parent = parent;
        ParentId = parent?.Id;
        _primaries = new List<PrimaryBarcode>();
    }

    public void AddPrimary(PrimaryBarcode primary)
    => _primaries.Add(primary);
}