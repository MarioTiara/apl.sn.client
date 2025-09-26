using SN.Core.Domain.Barcodes;
using SN.Core.Domain.Common;

namespace SN.Core.Domain.SAPIntegration;

public class SAPDataSyncLog : BaseEntity
{
    public Guid BarcodeId { get; private set; }
    public Barcode Barcode { get; private set; } 
    
    public bool IsSuccess { get; set; }
    public string? ResponseMessage { get; set; }

    public int RetryCount { get; private set; }
    public DateTime? LastRetryAt { get; private set; }

    protected SAPDataSyncLog() { }
    public SAPDataSyncLog(Barcode barcode)
    {
        BarcodeId = barcode.Id;
        Barcode = barcode;
        IsSuccess = false;
        RetryCount = 0;
        LastRetryAt = null;
    }

    public void MarkAsSuccess(string? responseMessage = null)
    {
        IsSuccess = true;
        ResponseMessage = responseMessage;
        this.RetryCount++;
        this.LastRetryAt = DateTime.UtcNow;
    }

    public void MarkAsFailed(string? responseMessage = null)
    {
        IsSuccess = false;
        ResponseMessage = responseMessage;
        this.RetryCount++;
        this.LastRetryAt = DateTime.UtcNow;
    }
}