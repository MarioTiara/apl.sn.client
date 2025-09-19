namespace SN.Core.Domain.ValueObjects;

public sealed class Batch
{
    public string? Value { get; }

    public Batch(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            // Basic normalization/validation placeholder — extend as needed
            Value = value.Trim();
        }
        else
        {
           
        }
    }

    public override string? ToString() => Value;
}