using SN.Core.Domain;
using SN.Core.Domain.ValueObjects;

namespace SN.Core.Factories;

public interface IBPOM2DBarcodeFactory
{
    IBPOM2DBarcode Create(Serial serial, AgregationLevel level, Gtin? gtin = null, Batch? batch = null, DateOnly? expireDate = null, DateOnly? manufactoringDate=null, RegistrationStatus registrationStatus = RegistrationStatus.Pending);
}
