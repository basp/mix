namespace Mix;

public class IndexRegister : Register
{
    public IndexRegister(string name)
        : base(name)
    {
    }

    public override int Byte1
    {
        get => 0;
        set { }
    }

    public override int Byte2
    {
        get => 0;
        set { }
    }

    public override int Byte3
    {
        get => 0;
        set { }
    }

    public override int Value
    {
        get => this.Sign > 0 ? -this.data[4, 5] : this.data[4, 5];
        set
        {
            this.Sign = value < 0 ? 1 : 0;
            this.data[4, 5] = Math.Abs(value);
        }
    }

    public override string ToString() =>
        string.Concat(
            nameof(IndexRegister),
            "{",
            this.name,
            ", ",
            this.data,
            "}");
}
