using SN.Core.Domain.Common;
using SN.Core.Domain.ValueObjects;

namespace SN.Core.Domain.Barcodes;


public class Barcode:BaseEntity
{
    public string BPOM2DBarCode { get; private set; } 
    public string? Gtin { get; private set; }
    public string Serial { get; private set; }
    public string? Batch { get; private set; }
    public DateOnly? ExpireDate { get; private set; }
    public AgregationLevel Level { get; private set; }

    public Barcode() { }
    public Barcode(IBPOM2DBarcode barcode)
    {
        BPOM2DBarCode = barcode.Get2DBarcode();
        Gtin = barcode.Gtin?.Value;
        Serial = barcode.Serial.Value;
        Batch = barcode.Batch?.Value;
        ExpireDate = barcode.ExpireDate;
        Level = barcode.Level;
    }
}