using SN.Core.Domain;
using SN.Core.Domain.ValueObjects;


namespace SN.Applications.Documents;

public class AggregationNode
{
    public string Id { get; private set; }
    public AgregationLevel Level { get; private set; }
    public List<AggregationNode> Children { get; private set; }

    public Serial SerialCode { get; private set; }
    public Gtin? GtinCode { get; private set; }
    public string? Parentid { get; private set; }
    public Batch? Batch { get; private set; }
    public DateOnly? ExpireDate { get; private set; }

    public DateOnly? ManufactoringDate { get; private set; }

    public AggregationNode(string id, Serial serialCode, Gtin? gtin, Batch? batch, DateOnly? expireDate, DateOnly? manufactoringDate = null, string? parentId = null) //, AgregationLevel level
    {
        Id = id;
        // Level = level;
        SerialCode = serialCode;
        GtinCode = gtin;
        Children = new List<AggregationNode>();
        Batch = batch;
        ExpireDate = expireDate;
        ManufactoringDate = manufactoringDate;
        Parentid = parentId;
        // Parentid = parentId;
    }

    public void AddChildren(AggregationNode node)
    => Children.Add(node);

    public void SetParentId(string parentID)
     => this.Parentid = parentID;

    public void SetLevel(AgregationLevel level)
      => this.Level = level; 

}