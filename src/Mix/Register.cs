namespace Mix;

/// <summary>
/// Represents a data register for a MIX computer.
/// </summary>
public class Register
{
    protected readonly string name;

    protected Word data = new Word();

    /// <summary>
    /// Initializes a new <see cref="Register"/> instance.
    /// </summary>
    /// <param name="name">The name for this register.</param>
    public Register(string name)
    {
        this.name = name;
    }

    /// <summary>
    /// Gets or sets the data for this register as a raw <see cref="Word"/>
    /// value.
    /// </summary>
    public virtual Word Data
    {
        get => this.data;
        set => this.data = value;
    }


    /// <summary>
    /// Gets or sets the sign for this register.
    /// </summary>
    /// <remarks>
    /// The sign field is exactly one bit long. It will be one when the value
    /// is negative and otherwise zero.
    /// </remarks>
    public virtual int Sign
    {
        get => this.data[0];
        set => this.data[0] = value;
    }

    /// <summary>
    /// Gets or sets the value of the first byte in this word.
    /// </summary>
    public virtual int Byte1
    {
        get => this.data[1];
        set => this.data[1] = value;
    }

    /// <summary>
    /// Gets or sets the value of the second byte in this word.
    /// </summary>
    public virtual int Byte2
    {
        get => this.data[2];
        set => this.data[2] = value;
    }

    /// <summary>
    /// Gets or sets the value of the third byte in this word.
    /// </summary>
    public virtual int Byte3
    {
        get => this.data[3];
        set => this.data[3] = value;
    }

    /// <summary>
    /// Gets or sets the value of the fourth byte in this word.
    /// </summary>
    public virtual int Byte4
    {
        get => this.data[4];
        set => this.data[4] = value;
    }

    /// <summary>
    /// Gets or sets the value of the fifth byte in this word.
    /// </summary>
    public virtual int Byte5
    {
        get => this.data[5];
        set => this.data[5] = value;
    }

    /// <summary>
    /// Gets or sets the complete register value as it is interpreted
    /// by the .NET runtime.
    /// </summary>
    /// <remarks>
    /// This will take the value of the bytes in this word return the
    /// value with respect to the sign bit. So if the sign bit is set
    /// then the contents of the register will be interpreted to be
    /// negative otherwise they will be positive. 
    /// </remarks>
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
