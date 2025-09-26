
using SN.Core.Domain.Documents;
using SN.Core.Domain.ValueObjects;

namespace SN.Core.Domain.Barcodes;

public class PrimaryBarcode : BarcodeAgregation
{
    //Navigate to Secondary Barcode
    public Guid? ParentId { get; private set; }
    public SecondaryBarcode? Parent { get; private set; }
    // Navigation property ke detail

    public Guid DetailId { get; private set; }
    public Barcode Detail { get; private set; }

    public PrimaryBarcode() { }

    public PrimaryBarcode(IBPOM2DBarcode barcode, SNDocument document, SecondaryBarcode? parent = null)
    {
        Detail = new Barcode(barcode);
        DetailId = Detail.Id;
        DocumentId = document.Id;
        BPOM2DBarCode = barcode.Get2DBarcode();
        ParentId = parent?.Id;
        Parent = parent;
        Document = document;
        RegistrationStatus = RegistrationStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }
    
}