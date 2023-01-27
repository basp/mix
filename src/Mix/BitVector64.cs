namespace Mix;

public struct BitVector64 : IEquatable<BitVector64>
{
    public readonly struct Section
    {
        private readonly int offset;

        private readonly long mask;

        public Section(int sizeInBits, int offset)
        {
            this.mask = ((1L << sizeInBits) - 1) << offset;
            this.offset = offset;
        }

        public long Mask => this.mask;

        public int Offset => this.offset;
    }

    private long data;

    public BitVector64(long data)
    {
        this.data = data;
    }

    public long this[Section section]
    {
        get => (this.data & section.Mask) >> section.Offset;
        set
        {
            this.data &= ~section.Mask;
            this.data |= (value << section.Offset);
        }
    }

    public long Data => this.data;

    public static Section CreateSection(int sizeInBits, int offset) =>
        new Section(sizeInBits, offset);

    public override string ToString() =>
        string.Concat(
            nameof(BitVector64),
            "{",
            Convert.ToString(this.data, 2).PadLeft(sizeof(long) * 8, '0'),
            "}");

    public bool Equals(BitVector64 other) =>
        this.data == other.data;
}