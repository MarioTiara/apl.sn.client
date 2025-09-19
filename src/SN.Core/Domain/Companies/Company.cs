using SN.Core.Domain.Common;

namespace SN.Core.Domain.Companies;

public class Company : BaseEntity, IAggregateRoot
{
    public string CompanyCode { get; private set; }
    public string CompanyName { get; private set; }
    public int? BpomSaranaId { get; private set; }
    public string? BpomUsername { get; private set; }
    public string? BpomPassword { get; private set; }
    public string? BpomToken { get; private set; }
    public DateTime? LastUpdatedToken { get; private set; }

    // For EF / serializer
    protected Company() { }

    public Company(string companyCode, string companyName)
    {
        CompanyCode = companyCode;
        CompanyName = companyName ?? throw new ArgumentNullException(nameof(companyName));
    }

    public void UpdateBpomCredentials(int? saranaId, string username, string password)
    {
        BpomSaranaId = saranaId;
        BpomUsername = username;
        BpomPassword = password;
    }

    public void UpdateToken(string token, DateTime updatedAt)
    {
        BpomToken = token;
        LastUpdatedToken = updatedAt;
    }
}

