namespace SN.Core.Domain.ValueObjects;

public sealed class Gtin
{
    public string Value { get; }

    public Gtin(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            // Basic normalization/validation placeholder — extend as needed
            Value = value.Trim();
        }
        else
        {
            throw new Exception("Gtin value is not match requirements");
        }
    }

    public override string? ToString() => Value;
}