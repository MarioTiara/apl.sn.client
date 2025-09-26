using System.Collections.Immutable;
using SN.Core.Domain.Documents;
using SN.Core.Domain.ValueObjects;

namespace SN.Core.Domain.Barcodes;

public class TertiaryBarcode : BarcodeAgregation
{
    private List<SecondaryBarcode> _secondaries;
    // Navigation property ke detail
    public Guid DetailId { get; private set; }
    public Barcode Detail { get; private set; }
    public IEnumerable<SecondaryBarcode> Secondaries => _secondaries.ToImmutableList().AsReadOnly();
    public TertiaryBarcode(){}
    public TertiaryBarcode(IBPOM2DBarcode barcode, SNDocument document)
    {
        DocumentId = document.Id;
        Document = document;
        RegistrationStatus = RegistrationStatus.Pending;
        BPOM2DBarCode = barcode.Get2DBarcode();
        Detail = new Barcode(barcode);
        DetailId = Detail.Id;
        RegistrationStatus = RegistrationStatus.Pending;
        _secondaries = new List<SecondaryBarcode>();
    }

    public void AddSecondary(SecondaryBarcode secondary)
     => _secondaries.Add(secondary);
}