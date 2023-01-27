namespace Mix;

using System.Diagnostics.CodeAnalysis;

public struct FieldSpec : IEquatable<FieldSpec>
{
    private readonly int value;

    /// <summary>
    /// Initializes a new <see cref="FieldSpec"/> based on an existing value.
    /// </summary>
    public FieldSpec(int value)
    {
        this.value = value;
    }

    /// <summary>
    /// Initializes a new <see cref="FieldSpec"/> based on a first and last
    /// field index of a MIX word.
    /// </summary>
    public FieldSpec(int first, int last)
    {
        this.value = (8 * first) + last;
    }

    /// <summary>
    /// Returns integer value of this field specification.
    /// </summary>
    public int Value => this.value;

    /// <summary>
    /// Deconstructs this field specification in a pair of first and last 
    /// values. They represent the start and end of the byte range that
    /// we want to address.
    /// </summary>
    public (int, int) Deconstruct()
    {
        var first = Math.DivRem(this.value, 8, out var last);
        return (first, last);
    }

    /// <inheritdoc/>
    public override string ToString() =>
        string.Concat(
            nameof(FieldSpec),
            "{",
            this.value,
            "}");

    /// <inheritdoc/>
    public override int GetHashCode() =>
        this.value.GetHashCode();

    /// <inheritdoc/>
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
