namespace Mix;

public class Simulator
{
    private Register[] indexRegisters;

    public Word[] Memory { get; } = new Word[4000];

    public Register A { get; } = new Register(nameof(A));

    public Register X { get; } = new Register(nameof(X));

    public IndexRegister I1 { get; } = new IndexRegister(nameof(I1));

    public IndexRegister I2 { get; } = new IndexRegister(nameof(I2));

    public IndexRegister I3 { get; } = new IndexRegister(nameof(I3));

    public IndexRegister I4 { get; } = new IndexRegister(nameof(I4));

    public IndexRegister I5 { get; } = new IndexRegister(nameof(I5));

    public IndexRegister I6 { get; } = new IndexRegister(nameof(I6));

    public JumpRegister J { get; } = new JumpRegister(nameof(J));

    public Simulator()
    {
        this.indexRegisters = new Register[]
        {
            // Add a ghost register so that the names of the index
            // registers properly map to their register index in 
            // the list of index registers (i.e. I1 = 1, I2 = 2, etc.)
            new DeadRegister(),
            I1,
            I2,
            I3,
            I4,
            I5,
            I6,
        };
    }

    public void LDA(int address, int i = 0, int first = 0, int last = 5)
    {
        this.A.Data = Load(address, i, first, last);
    }

    public void LDX(int address, int i = 0, int first = 0, int last = 5)
    {
        this.X.Data = Load(address, i, first, last);
    }

    public void LD1(int address, int i = 0, int first = 0, int last = 5) =>
        LDi(1, address, i, first, last);

    public void LD2(int address, int i = 0, int first = 0, int last = 5) =>
        LDi(2, address, i, first, last);

    public void LD3(int address, int i = 0, int first = 0, int last = 5) =>
        LDi(3, address, i, first, last);

    public void LD4(int address, int i = 0, int first = 0, int last = 5) =>
        LDi(4, address, i, first, last);

    public void LD5(int address, int i = 0, int first = 0, int last = 5) =>
        LDi(5, address, i, first, last);

    public void LD6(int address, int i = 0, int first = 0, int last = 5) =>
        LDi(6, address, i, first, last);

    public void LDAN(int address, int i = 0, int first = 0, int last = 5)
    {
        this.LDA(address, i, first, last);
        this.A.Sign = 1;
    }

    public void LDXN(int address, int i = 0, int first = 0, int last = 5)
    {
        this.LDX(address, i, first, last);
        this.X.Sign = 1;
    }

    public void LD1N(int address, int i = 0, int first = 0, int last = 5) =>
        LDiN(1, address, i, first, last);

    public void LD2N(int address, int i = 0, int first = 0, int last = 5) =>
        LDiN(2, address, i, first, last);

    public void LD3N(int address, int i = 0, int first = 0, int last = 5) =>
        LDiN(3, address, i, first, last);

    public void LD4N(int address, int i = 0, int first = 0, int last = 5) =>
        LDiN(4, address, i, first, last);

    public void LD5N(int address, int i = 0, int first = 0, int last = 5) =>
        LDiN(5, address, i, first, last);

    public void LD6N(int address, int i = 0, int first = 0, int last = 5) =>
        LDiN(6, address, i, first, last);

    public void STA(int address, int i = 0, int first = 0, int last = 5) =>
        this.Memory[address + i] = Store(this.A, address, i, first, last);

    public void STX(int address, int i = 0, int first = 0, int last = 5) =>
        this.Memory[address + i] = Store(this.X, address, i, first, last);

    private void LDiN(
        int register,
        int address,
        int i = 0,
        int first = 0,
        int last = 5)
    {
        var reg = this.indexRegisters[register];
        reg.Data = Load(address, i, first, last);
        reg.Sign = 1;
    }

    private void LDi(
        int register,
        int address,
        int i = 0,
        int first = 0,
        int last = 5)
    {
        var reg = this.indexRegisters[register];
        reg.Data = Load(address, i, first, last);
    }

    private Word Store(
        Register reg,
        int address,
        int i = 0,
        int first = 0,
        int last = 5)
    {
        var w = reg.Data;
        var lshift = first;
        var v = new Word();

        if (first == 0)
        {
            v[0] = w[0];
            first += 1;
        }

        for (var j = first; j <= last; j++)
        {
            v[j - lshift] = w[j];
        }

        return v;
    }

    private Word Load(
        int address,
        int i = 0,
        int first = 0,
        int last = 5)
    {
        var w = this.Memory[address + i];
        var rshift = 5 - last;
        var v = new Word();

        if (first == 0)
        {
            // If the sign field (0) is part of the field specification then
            // we want to always copy it as well. However, bytes that make up
            // the actual value of word need to be shifted to the right before
            // we load them in into the register. The sign field should be
            // excluded from this shifting operation.
            v[0] = w[0];
            first += 1;
        }

        // Shift all the bytes `rshift` fields to the right.
        for (var j = first; j <= last; j++)
        {
            v[j + rshift] = w[j];
        }

        return v;
    }
}