using SN.Core.Domain;
using SN.Core.Domain.ValueObjects;

namespace SN.AZIParser.Commons;

public class AZI2DBarcode : IBPOM2DBarcode
{
    public Serial Serial { get; private set; }

    public Gtin? Gtin { get; private set; }

    public Batch? Batch { get; private set; }

    public DateOnly? ExpireDate { get; private set; }

    public AgregationLevel Level { get; private set; }

    public RegistrationStatus RegistrationStatus  { get; private set; }

    public DateOnly? ManufactoringDate { get; private set; }

    public AZI2DBarcode(Serial serial, AgregationLevel level, Gtin? gtin = null, Batch? batch = null, DateOnly? expireDate = null, DateOnly? manufactoringDate=null, RegistrationStatus registrationStatus = RegistrationStatus.RegisteredExternally)
    {
        Serial = serial;
        Gtin = gtin;
        Batch = batch;
        ExpireDate = expireDate;
        Level = level;
        RegistrationStatus = registrationStatus;
    }

    public string Get2DBarcode()
    {
        if (Level == AgregationLevel.Primary && Gtin is not null && Batch is not null && ExpireDate is not null)
            return $"01{Gtin}21{Serial}17{ExpireDate:yyMMdd}10{Batch}";
        if (Level != AgregationLevel.Primary && Gtin is not null)
            return $"01{Gtin}21{Serial}";
        return Serial.ToString();
    }
}

