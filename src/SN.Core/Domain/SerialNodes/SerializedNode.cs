
using SN.Core.Domain.Common;

namespace SN.Core.Domain.SerialNode;

public class SerializedNode:BaseEntity 
{
    public string SerializedCode { get; private set; }
    public AgregationLevel Level { get; private set; }
    public Guid?  ParentId { get; private set; }
    public Guid DocumentId { get; private set; }
    protected SerializedNode() { }

    public SerializedNode(string serializedCode, AgregationLevel level, Guid documentId, Guid? parentId = null)
    {
        SerializedCode = serializedCode;
        Level = level;
        DocumentId = documentId;
        CreatedAt = DateTime.UtcNow;
        ParentId = parentId;
    }
}

