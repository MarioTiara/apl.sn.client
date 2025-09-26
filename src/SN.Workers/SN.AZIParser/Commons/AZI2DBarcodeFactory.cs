using SN.Core.Domain;
using SN.Core.Domain.ValueObjects;
using SN.Core.Factories;

namespace SN.AZIParser.Commons;

public class AZI2DBarcodeFactory : IBPOM2DBarcodeFactory
{
    public IBPOM2DBarcode Create(Serial serial, AgregationLevel level, Gtin? gtin = null, Batch? batch = null, DateOnly? expireDate = null, RegistrationStatus registrationStatus = RegistrationStatus.RegisteredExternally)
    {
        return new AZI2DBarcode(serial, level, gtin, batch, expireDate, registrationStatus);
    }
}