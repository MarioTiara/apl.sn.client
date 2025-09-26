namespace SN.Core.Domain.ValueObjects;

public sealed class Batch
{
    public string Value { get; }

    public Batch(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            // Basic normalization/validation placeholder — extend as needed
            Value = value.Trim();
        }
        else
        {
            throw new Exception("Batch value is not match requirements");
        }
    }

    public override string ToString() => Value;
}