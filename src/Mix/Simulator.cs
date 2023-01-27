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
        // Add a *dead* register so that the names of the index
        // registers properly map to their register index in 
        // the list of index registers (i.e. I1 = 1, I2 = 2, etc.).
        // 
        // This dead register should be ignored by any instructions.
        // It is incapable of storing any value - its bits will always be 
        // zero.
        var dead = new DeadRegister();
        this.indexRegisters = new Register[]
        {
            dead,   // 000 
            I1,     // I=1
            I2,     // I=2
            I3,     // I=3
            I4,     // I=4
            I5,     // I=5
            I6,     // I=6
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

        // As with the `Store` method we deal with the first field if it
        // includes the sign bit. If `first` is not the sign field then we
        // just ignore it and assume positive (0).
        if (first == 0)
        {
            v[0] = w[0];
            first += 1;
        }

        // Instead of shifting to the right we now need to left shift our
        // values from ram into the register. This is important so that when
        // we load previously stored values from memory using the same field
        // index we can assume the same results instead of having to shift
        // our words around.
        // 
        // When we *LOAD* we need to *right align* the results *before* loading 
        // them into memory but when we *STORE* results from memory they are
        // left aligned *before* they are loaded into the register.
        // store we 
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