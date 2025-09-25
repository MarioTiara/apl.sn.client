namespace SN.Infrastructure.EPCIS;

public interface IAggregationEvent
{
    string? ParentId { get; }
    List<string>? ChildEpcs { get; }
}