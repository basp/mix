namespace Mix;

using System.Diagnostics.CodeAnalysis;

public struct FieldSpec : IEquatable<FieldSpec>
{
    private readonly int value;

    public FieldSpec(int value)
    {
        this.value = value;
    }

    public FieldSpec(int first, int last)
    {
        this.value = (8 * first) + last;
    }

    public int Value => this.value;

    public (int, int) Deconstruct()
    {
        var first = Math.DivRem(this.value, 8, out var last);
        return (first, last);
    }

    public override string ToString() =>
        string.Concat(
            nameof(FieldSpec),
            "{",
            this.value,
            "}");

    public override int GetHashCode() =>
        this.value.GetHashCode();

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is FieldSpec other)
        {
            return this.value == other.value;
        }

        return false;
    }

    public static bool operator !=(FieldSpec x, FieldSpec y) =>
        x.value != y.value;

    public static bool operator ==(FieldSpec x, FieldSpec y) =>
        x.value == y.value;

    public bool Equals(FieldSpec other) =>
        this.value == other.value;
}
