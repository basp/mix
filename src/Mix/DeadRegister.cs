namespace Mix;

public class DeadRegister : Register
{
    public DeadRegister()
        : base("0xDEAD")
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

    public override int Byte4
    {
        get => 0;
        set { }
    }

    public override int Byte5
    {
        get => 0;
        set { }
    }

    public override Word Data => new Word(0);

    public override int Value
    {
        get => 0;
        set { }
    }

    public override string ToString() =>
        string.Concat(
            nameof(DeadRegister),
            "{",
            this.data,
            "}");

    public override bool Equals(object? obj)
    {
        if (obj is DeadRegister other)
        {
            return true;
        }

        return false;
    }

    public override int GetHashCode() =>
        this.data.GetHashCode();
}