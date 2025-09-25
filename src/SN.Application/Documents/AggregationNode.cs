using SN.Core.Domain;
using SN.Core.Domain.ValueObjects;


namespace SN.Applications.Documents;

public class AggregationNode
{
    public string Id { get; private set; }
    public AgregationLevel Level { get; private set; }
    public List<AggregationNode> Children { get; private set; }

    public Serial SerialCode { get; private set; }
    public Gtin GtinCode { get; private set; }
    public string? Parentid { get; private set; }
    public string? Batch { get; private set; }
    public string? ExpireDate { get; private set; }

    public AggregationNode(string id, Serial serialCode, Gtin gtin, string? batch, string? expireDate)
    {
        Id = id;
        // Level = level;
        SerialCode = serialCode;
        GtinCode = gtin;
        Children = new List<AggregationNode>();
        Batch = batch;
        ExpireDate = expireDate;
        // Parentid = parentId;
    }

    public void AddChildren(AggregationNode node)
    => Children.Add(node);

    public void SetParentId(string parentID)
     => this.Parentid = parentID;

    public void SetLevel(AgregationLevel level)
      => this.Level = level; 

}