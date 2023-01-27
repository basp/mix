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

    /// <summary>
    /// Initializes a new <see cref="Word"/> instance using existing data.
    /// </summary>
    public Word(int data)
    {
        this.bits = new BitVector32(data);
    }

    /// <summary>
    /// Gets the field value of this word at given index.
    /// </summary>
    public int this[int index]
    {
        get => this.bits[fields[index].Section];
        set => this.bits[fields[index].Section] = value;
    }

    /// <summary>
    /// Gets or sets the value of this word in the fields from first to last.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Reading a field range always results in an *aligned* result. So that if 
    /// you read the first two bytes of a value (as a section) then those two 
    /// bytes will be shifted so that they align with the right of the 
    /// <see cref="Word"/>.
    /// </para>
    /// <para>
    /// In effect his means you can store integer values in sections within a 
    /// word and they *will* be preserved correctly when you read them.
    /// </para>
    /// </remarks>
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

    /// <summary>
    /// Gets or sets the value of this word based on a field specification.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the value to be written is greater than would be allowed by the 
    /// given <see cref="FieldSpec"/> value values might get clobbered over.
    /// </para>
    /// <para>
    /// Clients should be aware of the size of registers and memory cells and
    /// their capabilities and restrictions. If the simulator is asked to do 
    /// something which might seem unreasonable for a human it will hapilly
    /// comply in a some way. This may or may not be the correct way.
    /// </para>
    /// </remarks>
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

    /// <summary>
    /// Gets a value indicating whether all the bits in this <see cref="Word"/>
    /// instance are zero.
    /// </summary>
    public bool IsZero => this.bits.Data == 0;

    /// <summary>
    /// Gets the sign of this <see cref="Word"/> instance.
    /// </summary>
    /// <remarks>
    /// The sign field is a one bit value that is set to one (1) when the value is
    /// negative. Otherwise it is set to zero (0).
    /// </remarks>
    public int Sign
    {
        get => this.bits[fields[0].Section];
        set => this.bits[fields[0].Section] = value;
    }

    /// <summary>
    /// Gets the value of the first byte in this <see cref="Word"/> instance.
    /// </summary>
    public int Byte1
    {
        get => this.bits[fields[1].Section];
        set => this.bits[fields[1].Section] = value;
    }

    /// <summary>
    /// Gets the value of the second byte in this <see cref="Word"/> instance.
    /// </summary>
    public int Byte2
    {
        get => this.bits[fields[2].Section];
        set => this.bits[fields[2].Section] = value;
    }

    /// <summary>
    /// Gets the value of the third byte in this <see cref="Word"/> instance.
    /// </summary>
    public int Byte3
    {
        get => this.bits[fields[3].Section];
        set => this.bits[fields[3].Section] = value;
    }

    /// <summary>
    /// Gets the value of the fourth byte in this <see cref="Word"/> instance.
    /// </summary>
    public int Byte4
    {
        get => this.bits[fields[4].Section];
        set => this.bits[fields[4].Section] = value;
    }

    /// <summary>
    /// Gets the value of the fifth byte in this <see cref="Word"/> instance.
    /// </summary>
    public int Byte5
    {
        get => this.bits[fields[5].Section];
        set => this.bits[fields[5].Section] = value;
    }

    /// <summary>
    /// Gets the integer value of this <see cref="Word"/> instance.
    /// </summary>
    public int Value => this.bits.Data;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        this.Value.GetHashCode();

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Word other)
        {
            return this.Equals(other);
        }

        return false;
    }

    /// <inheritdoc/>
    public override string ToString() =>
        string.Concat(
            nameof(Word),
            "{",
            this.bits,
            "}");

    /// <inheritdoc/>
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
