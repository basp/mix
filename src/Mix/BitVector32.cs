using System.Diagnostics.CodeAnalysis;

namespace Mix;

public struct BitVector32 : IEquatable<BitVector32>
{
	public readonly struct Section
	{
		private readonly int offset;

		private readonly int mask;

		public Section(int sizeInBits, int offset)
		{
			this.mask = ((1 << sizeInBits) - 1) << offset;
			this.offset = offset;
		}

        public int Mask => this.mask;

        public int Offset => this.offset;
	}
	
	private int data;
	
	public BitVector32(int data)
	{
		this.data = data;
	}
	
	public int this[Section section]
	{
		get => (this.data & section.Mask) >> section.Offset;
		set
		{
            this.data &= ~section.Mask;
            this.data |= (value << section.Offset);
		}
	}

    public int Data => this.data;

    public static bool operator !=(BitVector32 x, BitVector32 y) =>
        x.data != y.data;

    public static bool operator ==(BitVector32 x, BitVector32 y) =>
        x.data == y.data;

    public static Section CreateSection(int sizeInBits, int offset) =>
        new Section(sizeInBits, offset);

	public override string ToString() =>
        string.Concat(
            nameof(BitVector32),
            "{",
		    Convert.ToString(this.data, 2).PadLeft(sizeof(int) * 8, '0'),
            "}");

    public override int GetHashCode() =>
        this.data.GetHashCode();

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is BitVector32 other)
        {
            return this.data == other.data;
        }

        return false;
    }
    
    public bool Equals(BitVector32 other) =>
        this.data == other.data;
}