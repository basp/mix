namespace Mix;

public class Register
{
    protected readonly string name;

    protected Word data = new Word();

    public Register(string name)
    {
        this.name = name;
    }

    public virtual Word Data
    {
        get => this.data;
        set => this.data = value;
    }

    public virtual int Sign
    {
        get => this.data[0];
        set => this.data[0] = value;
    }

    public virtual int Byte1
    {
        get => this.data[1];
        set => this.data[1] = value;
    }

    public virtual int Byte2
    {
        get => this.data[2];
        set => this.data[2] = value;
    }

    public virtual int Byte3
    {
        get => this.data[3];
        set => this.data[3] = value;
    }

    public virtual int Byte4
    {
        get => this.data[4];
        set => this.data[4] = value;
    }

    public virtual int Byte5
    {
        get => this.data[5];
        set => this.data[5] = value;
    }

    public virtual int Value
    {
        get => this.Sign > 0 ? -this.data[1, 5] : this.data[1, 5];
        set
        {
            this.Sign = value < 0 ? 1 : 0;
            this.data[1, 5] = Math.Abs(value);
        }
    }

    public override string ToString() =>
        string.Concat(
            nameof(Register),
            "{",
            this.name,
            ", ",
            this.data,
            "}");
}
