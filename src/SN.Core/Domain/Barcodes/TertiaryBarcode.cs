using System.Collections.Immutable;
using SN.Core.Domain.ValueObjects;

namespace SN.Core.Domain.Barcodes;

public class TertiaryBarcode : BarcodeAgregation
{
    private List<SecondaryBarcode> _secondaries;
    public Barcode Detail { get; private set; }
    public IEnumerable<SecondaryBarcode> Secondaries => _secondaries.ToImmutableList().AsReadOnly();
    public TertiaryBarcode(){}
    public TertiaryBarcode(BPOM2DBarcode barcode, Guid documentId)
    {
        DocumentId = documentId;
        BPOM2DBarCode = barcode.ToString();
        RegistrationStatus = RegistrationStatus.Pending;
        BPOM2DBarCode = barcode.ToString();
        Detail = new Barcode(barcode);
        RegistrationStatus = RegistrationStatus.Pending;
        _secondaries = new List<SecondaryBarcode>();
    }

    public void AddSecondary(SecondaryBarcode secondary)
     => _secondaries.Add(secondary);
}