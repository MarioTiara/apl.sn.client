

namespace SN.Core.Domain.ValueObjects;

public interface IBPOM2DBarcode
{
    Serial Serial { get; }
    Gtin? Gtin { get; }
    Batch? Batch { get; }
    DateOnly? ExpireDate { get; }

    AgregationLevel Level { get; }

    RegistrationStatus RegistrationStatus { get; }

    string Get2DBarcode();
}

