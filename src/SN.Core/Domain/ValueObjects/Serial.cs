namespace SN.Core.Domain.ValueObjects;

public sealed class Serial
{
    public string Value { get; }

    public Serial(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            // Basic normalization/validation placeholder — extend as needed
            Value = value.Trim();
        }
        else
        {
            throw new Exception("Serial value is not match requirements");
        }
    }

    public override string? ToString() => Value;
}