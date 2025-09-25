using SN.Applications.Documents;
using SN.Core.Domain;
using SN.Core.Domain.ValueObjects;

namespace SN.Infrastructure.EPCIS;

public class EPCISAgregationBuilder : IAggregationBuilder
{
    private List<IAggregationEvent> _events;
    private Dictionary<string, EPCISAttribute> _epcisAttrubuteMaps;
    public EPCISAgregationBuilder(IEnumerable<IAggregationEvent> events, Dictionary<string, EPCISAttribute> attributesMap)
    {
        _events = events.ToList();
        _epcisAttrubuteMaps = attributesMap;
    }
    public List<AggregationNode> Build()
    {
        // Lookup tables
        var parentToChildren = _events
            .Where(e => e.ChildEpcs != null && e.ChildEpcs.Any())
            .ToDictionary(
                e => e.ParentId!,
                e => e.ChildEpcs!
            );

        var allChildren = parentToChildren.Values.SelectMany(c => c).ToHashSet();
        var allParents = parentToChildren.Keys.ToHashSet();

        // Roots = parents that are not children
        var roots = allParents.Where(p => !allChildren.Contains(p)).ToList();

        var result = new List<AggregationNode>();
        foreach (var root in roots)
        {
            result.Add(BuildNode(root, parentToChildren, null));
        }
        return result;
    }

    private AggregationNode BuildNode(string id, Dictionary<string, List<string>> parentToChildren, string? parentId)
    {
        var serialEpcis = EPCISParser.GetSerialCodefromEPCCode(id) ?? string.Empty;
        var gtinEpcis = EPCISParser.GetGTINfromEPCCode(id) ?? string.Empty;

        var serial = new Serial(serialEpcis);
        var gtin = new Gtin(gtinEpcis);

        _epcisAttrubuteMaps.TryGetValue(id, out var attribute);

        var node = new AggregationNode(
            id,
            serial,
            gtin,
            attribute?.LOTNO,
            attribute?.DATEX
        );

        // Build children recursively
        if (parentToChildren.TryGetValue(id, out var children))
        {
            foreach (var child in children)
            {
                node.AddChildren(BuildNode(child, parentToChildren, id));
            }
        }

        // Set parent
        if (!string.IsNullOrEmpty(parentId))
            node.SetParentId(parentId);

        // Determine level based on depth
        node.SetLevel(GetNodeLevel(node));

        return node;
    }

    private AgregationLevel GetNodeLevel(AggregationNode node)
    {
        if (!node.Children.Any())
            return AgregationLevel.Primary;

        // Find maximum depth of children
        int maxChildDepth = node.Children.Max(c => GetDepth(c));

        if (maxChildDepth == 1)
            return AgregationLevel.Secondary; // Two-level tree
        else
            return AgregationLevel.Tertiary; // Three-level tree or more
    }

    private int GetDepth(AggregationNode node)
    {
        if (!node.Children.Any())
            return 1;
        return 1 + node.Children.Max(c => GetDepth(c));
    }

}