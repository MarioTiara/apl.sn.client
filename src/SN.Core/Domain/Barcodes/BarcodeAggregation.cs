
using SN.Core.Domain.Common;
using SN.Core.Domain.Documents;

namespace SN.Core.Domain.Barcodes;

public abstract class BarcodeAgregation : BaseEntity
{
    public string BPOM2DBarCode { get; protected set; }
    public Guid DocumentId { get; protected set; }
    public SNDocument Document { get; protected set; } = null!;
    public RegistrationStatus RegistrationStatus { get; protected set; }
    protected BarcodeAgregation()
    {
        BPOM2DBarCode = string.Empty;
        RegistrationStatus = RegistrationStatus.Pending;
    }

    public void SetRegistrationStatus(RegistrationStatus status)
    {
        RegistrationStatus = status;
    }

}