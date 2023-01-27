using System.Diagnostics.CodeAnalysis;

namespace Mix;

public struct Word : IEquatable<Word>
{
    class Field
    {
        private readonly int sizeInBits;

        private readonly BitVector32.Section section;

        public Field(int sizeInBits, int offset)
        {
            this.sizeInBits = sizeInBits;
            this.section = BitVector32.CreateSection(sizeInBits, offset);
        }

        public int SizeInBits => this.sizeInBits;

        public int Offset => this.section.Offset;

        public int Mask => this.section.Mask;

        public BitVector32.Section Section => this.section;
    }

    private static readonly Field[] fields =
        new []
        {
            new Field(sizeInBits: 1, offset: 0),
            new Field(sizeInBits: 6, offset: 1),
            new Field(sizeInBits: 6, offset: 7),
            new Field(sizeInBits: 6, offset: 13),
            new Field(sizeInBits: 6, offset: 19),
            new Field(sizeInBits: 6, offset: 25),            
        };

    private BitVector32 bits;

    public Word(int data)
    {
        this.bits = new BitVector32(data);
    }

    public int this[int index]
    {
        get => this.bits[fields[index].Section];
        set => this.bits[fields[index].Section] = value;
    }

    public int this[int first, int last]
    {
        get
        {
            var section = CreateSection(first, last);
            return this.bits[section];
        }
        set
        {
            var section = CreateSection(first, last);
            this.bits[section] = value;
        }
    }

    public int this[FieldSpec spec]
    {
        get
        {
            var section = CreateSection(spec);
            return this.bits[section];
        }
        set
        {
            var section = CreateSection(spec);
            this.bits[section] = value;
        }
    }

    public bool IsZero => this.bits.Data == 0;

    public int Sign => this.bits[fields[0].Section];

    public int Byte1 => this.bits[fields[1].Section];

    public int Byte2 => this.bits[fields[2].Section];

    public int Byte3 => this.bits[fields[3].Section];

    public int Byte4 => this.bits[fields[4].Section];

    public int Byte5 => this.bits[fields[5].Section];

    public int Value => this.bits.Data;

    public override int GetHashCode() =>
        this.Value.GetHashCode();

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Word other)
        {
            return this.Equals(other);
        }

        return false;
    }

    public override string ToString() =>
        string.Concat(
            nameof(Word),
            "{",
            this.bits,
            "}");

    public bool Equals(Word other) =>
        this.bits == other.bits;

    public static bool operator !=(Word x, Word y) =>
        x.bits != y.bits;

    public static bool operator ==(Word x, Word y) =>
        x.bits == y.bits;

    private static BitVector32.Section CreateSection(int first, int last)
    {
        var count = last - first + 1;
        var offset = fields[first].Offset;
        var sizeInBits = Enumerable
            .Range(first, count)
            .Select(i => fields[i].SizeInBits)
            .Sum();
        return BitVector32.CreateSection(sizeInBits, offset);
    }

    private static BitVector32.Section CreateSection(FieldSpec spec)
    {
        var (first, last) = spec.Deconstruct();
        return CreateSection(first, last);
    }
}