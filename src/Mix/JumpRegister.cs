namespace Mix;

public class JumpRegister : Register
{
    public JumpRegister(string name)
        : base(name)
    {
    }

    public override int Sign
    {
        get => 0;
        set { }
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
        get => this.data[4, 5];
        set => this.data[4, 5] = Math.Abs(value);
    }

    public override string ToString() =>
        string.Concat(
            nameof(JumpRegister),
            "{",
            this.name,
            ", ",
            this.data,
            "}");
}