
using EPCIS.DTO;
using SN.Infrastructure.EPCIS;

namespace SN.AZIParser;

public class AggregationEventV1Adapter : IAggregationEvent
{
    private readonly AggregationEvent _event;
    public AggregationEventV1Adapter(AggregationEvent ev) => _event = ev;

    public string? ParentId => _event.ParentID;
    public List<string>? ChildEpcs => _event.ChildEPCs;
}
