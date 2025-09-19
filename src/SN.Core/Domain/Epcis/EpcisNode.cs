using SN.Core.Domain.Common;

namespace SN.Core.Domain.Epcis;

public class EpcisNode:BaseEntity 
{
    public string EpcisCode { get; private set; }
    public AgregationLevel Level { get; private set; }
    public Guid? ParentId { get; private set; }
    public Guid DocumentId { get; private set; }
    
    protected EpcisNode() { }

    public EpcisNode(string epcisCode, AgregationLevel level, Guid documentId, Guid? parentId = null)
    {
        EpcisCode = !string.IsNullOrWhiteSpace(epcisCode) ? epcisCode : throw new ArgumentNullException(nameof(epcisCode));
        Level = level;
        DocumentId = documentId;
        ParentId = parentId;
    }


}

