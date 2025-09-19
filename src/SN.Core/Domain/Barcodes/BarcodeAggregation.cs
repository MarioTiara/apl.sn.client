using System.Collections.Immutable;
using SN.Core.Domain.Common;

namespace SN.Core.Domain.Barcodes;

public abstract class BarcodeAgregation : BaseEntity
{
    public string BPOM2DBarCode { get; protected set; }
    public Guid DocumentId { get; protected set; }
    public RegistrationStatus RegistrationStatus { get; protected set; }
    protected BarcodeAgregation()
    {
        BPOM2DBarCode = string.Empty;
        RegistrationStatus = RegistrationStatus.Pending;
    }


}