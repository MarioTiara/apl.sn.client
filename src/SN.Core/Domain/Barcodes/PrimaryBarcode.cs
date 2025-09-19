
using SN.Core.Domain.ValueObjects;

namespace SN.Core.Domain.Barcodes;

public class PrimaryBarcode:BarcodeAgregation
{
    //Navigate to Secondary Barcode
    public Guid? ParentId { get; private set; }
    public SecondaryBarcode? Parent { get; private set; }
    // Navigation property ke detail
    public Barcode Detail { get; private set; }
    
    public PrimaryBarcode(){}

    public PrimaryBarcode(BPOM2DBarcode barcode, Guid documentId, SecondaryBarcode? parent = null)
    {
        Detail = new Barcode(barcode);
        DocumentId = documentId;
        BPOM2DBarCode = barcode.ToString();
        ParentId = parent?.Id;
        Parent = parent;
        DocumentId = documentId;
        RegistrationStatus = RegistrationStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }
}