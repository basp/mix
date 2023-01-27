using System.Diagnostics.CodeAnalysis;

namespace Mix;

public struct Instruction : IEquatable<Instruction>
{
    private Word data;

    public Instruction(Word data)
    {
        this.data = data;
    }

    public Word Data => this.data;

    public int Sign
    {
        get => this.data[0];
        set => this.data[0] = value;
    }

    public int AA
    {
        get => this.data[1, 2];
        set => this.data[1, 2] = value;
    }

    public int I
    {
        get => this.data[3];
        set => this.data[3] = value;
    }

    public int F
    {
        get => this.data[4];
        set => this.data[4] = value;
    }

    public int C
    {
        get => this.data[5];
        set => this.data[5] = value;
    }

    public int Address => this.Sign > 0 ? -this.AA : this.AA;

    public FieldSpec Fields
    {
        get => new FieldSpec(this.F);
        set => this.F = value.Value;
    }

    public static bool operator !=(Instruction x, Instruction y) =>
        !x.Equals(y);

    public static bool operator ==(Instruction x, Instruction y) =>
        x.Equals(y);

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Instruction other)
        {
            return this.Equals(other);
        }

        return false;
    }

    public override int GetHashCode() =>
        this.data.GetHashCode();

    public override string ToString() =>
        string.Concat(
            nameof(Instruction),
            "{",
            string.Join(
                ", ",
                new[]
                {
                    $"S={this.Sign}",
                    $"AA={this.AA}",
                    $"F={this.F}",
                    $"I={this.I}",
                    $"C={this.C}",
                }),
            "}");

    public bool Equals(Instruction other) =>
        this.data == other.data;
}