namespace SN.Core.Domain.ValueObjects;

public sealed class BPOM2DBarcode
{
    public Serial Serial { get; private set; }
    public Gtin Gtin { get; private set; }
    public Batch? Batch { get; private set; }
    public DateOnly? ExpireDate { get; private set; }

    public BPOM2DBarcode(Serial serial, Gtin gtin, Batch? batch = null, DateOnly? expireDate = null)
    {
        this.Serial = serial;
        this.Gtin = gtin;
        this.Batch = batch;
        this.ExpireDate = expireDate;
    }

    public override string ToString()
    {
        if (Batch == null || ExpireDate == null)
        {
            return $"01{this.Gtin.Value}21{this.Serial.Value}";
        }
        
        return $"01{this.Gtin.Value}10{this.Batch.Value}17{ExpireDate}21{this.Serial.Value}";
    }
}