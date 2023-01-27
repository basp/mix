namespace Mix.Tests;

using System;
using Xunit;

public class SimulatorTests
{
    [Fact]
    public void TestSimulatorConstruction()
    {
        var sim = new Simulator();
        Assert.All(sim.Memory, w => Assert.True(w.IsZero));
        Assert.True(sim.A.Data.IsZero);
        Assert.True(sim.X.Data.IsZero);
    }

    [Fact]
    public void TestFieldSpecLDA()
    {
        TestRegister(
            (sim, address, first, last) => sim.LDA(address, 0, first, last),
            sim => sim.A);
    }

    [Fact]
    public void TestFieldSpecLDX()
    {
        TestRegister(
            (sim, address, first, last) => sim.LDX(address, 0, first, last),
            sim => sim.X);
    }

    [Fact]
    public void TestFieldSpecLD1()
    {
        TestRegister(
            (sim, address, first, last) => sim.LD1(address, 0, first, last),
            sim => sim.I1);
    }

    [Fact]
    public void TestFieldSpecLD2()
    {
        TestRegister(
            (sim, address, first, last) => sim.LD2(address, 0, first, last),
            sim => sim.I2);
    }

    [Fact]
    public void TestFieldSpecLD3()
    {
        TestRegister(
            (sim, address, first, last) => sim.LD3(address, 0, first, last),
            sim => sim.I3);
    }

    [Fact]
    public void TestFieldSpecLD4()
    {
        TestRegister(
            (sim, address, first, last) => sim.LD4(address, 0, first, last),
            sim => sim.I4);
    }

    [Fact]
    public void TestFieldSpecLD5()
    {
        TestRegister(
            (sim, address, first, last) => sim.LD5(address, 0, first, last),
            sim => sim.I5);
    }

    [Fact]
    public void TestFieldSpecLD6()
    {
        TestRegister(
            (sim, address, first, last) => sim.LD6(address, 0, first, last),
            sim => sim.I6);
    }

    [Fact]
    public void TestFieldSpecLDAN()
    {
        TestLoadN(
            (sim, address, first, last) => sim.LDAN(address, 0, first, last),
            sim => sim.A);
    }

    [Fact]
    public void TestFieldSpecLDXN()
    {
        TestLoadN(
            (sim, address, first, last) => sim.LDXN(address, 0, first, last),
            sim => sim.X);
    }

    [Fact]
    public void TestFieldSpecLD1N()
    {
        TestIndexLoadN(
            (sim, address, first, last) => sim.LD1N(address, 0, first, last),
            sim => sim.I1);
    }

    [Fact]
    public void TestFieldSpecLD2N()
    {
        TestIndexLoadN(
            (sim, address, first, last) => sim.LD2N(address, 0, first, last),
            sim => sim.I2);
    }

    [Fact]
    public void TestFieldSpecLD3N()
    {
        TestIndexLoadN(
            (sim, address, first, last) => sim.LD3N(address, 0, first, last),
            sim => sim.I3);
    }

    [Fact]
    public void TestFieldSpecLD4N()
    {
        TestIndexLoadN(
            (sim, address, first, last) => sim.LD4N(address, 0, first, last),
            sim => sim.I4);
    }

    [Fact]
    public void TestFieldSpecLD5N()
    {
        TestIndexLoadN(
            (sim, address, first, last) => sim.LD5N(address, 0, first, last),
            sim => sim.I5);
    }

    [Fact]
    public void TestFieldSpecLD6N()
    {
        TestIndexLoadN(
            (sim, address, first, last) => sim.LD6N(address, 0, first, last),
            sim => sim.I6);
    }

    [Fact]
    public void TestDefaultSTA()
    {
        TestStoreRegister(
            (sim, address, first, last) => sim.STA(address, 0, first, last),
            sim => sim.A);
    }

    [Fact]
    public void TestDefaultSTX()
    {
        TestStoreRegister(
            (sim, address, first, last) => sim.STX(address, 0, first, last),
            sim => sim.X);
    }

    [Fact]
    public void TestFieldSpecST1()
    {

    }

    private void TestStoreRegister(
        Action<Simulator, int, int, int> store,
        Func<Simulator, Register> selectRegister)
    {
        var sim = new Simulator();
        sim.Memory[2000][0] = 1;
        sim.Memory[2000][1] = 1;
        sim.Memory[2000][2] = 2;
        sim.Memory[2000][3] = 3;
        sim.Memory[2000][4] = 4;
        sim.Memory[2000][5] = 5;

        var reg = selectRegister(sim);

        reg.Sign = 0;
        reg.Byte1 = 6;
        reg.Byte2 = 7;
        reg.Byte3 = 8;
        reg.Byte4 = 9;
        reg.Byte5 = 0;

        store(sim, 2000, 0, 5);

        Assert.Equal(reg.Data, sim.Memory[2000]);        
    }

    private void TestIndexLoadN(
        Action<Simulator, int, int, int> loadN,
        Func<Simulator, IndexRegister> selectRegister)
    {
        var sim = new Simulator();
        sim.Memory[2000][0, 0] = 0;
        sim.Memory[2000][1, 1] = 1;
        sim.Memory[2000][2, 2] = 2;
        sim.Memory[2000][3, 3] = 3;
        sim.Memory[2000][4, 5] = 80;

        var reg = selectRegister(sim);

        loadN(sim, 2000, 0, 2);

        var expected = sim.Memory[2000][1, 2];
        var actual = reg.Data[4, 5];

        Assert.Equal(1, reg.Sign);
        Assert.Equal(expected, actual);
    }

    private void TestLoadN(
        Action<Simulator, int, int, int> loadN,
        Func<Simulator, Register> selectRegister)
    {
        var sim = new Simulator();
        sim.Memory[2000][0, 0] = 0;
        sim.Memory[2000][1, 1] = 1;
        sim.Memory[2000][2, 2] = 2;
        sim.Memory[2000][3, 3] = 3;
        sim.Memory[2000][4, 5] = 80;

        var reg = selectRegister(sim);

        loadN(sim, 2000, 0, 5);

        var expected = sim.Memory[2000][1, 5];
        var actual = reg.Data[1, 5];

        Assert.Equal(1, reg.Sign);
        Assert.Equal(expected, actual);
    }

    private void TestIndexRegister(
        Action<Simulator, int, int, int> load,
        Func<Simulator, IndexRegister> selectRegister)
    {
        var sim = new Simulator();
        sim.Memory[2000][0, 0] = 1;
        sim.Memory[2000][1, 2] = 80;
        sim.Memory[2000][3, 3] = 3;
        sim.Memory[2000][4, 4] = 5;
        sim.Memory[2000][5, 5] = 4;

        var r = selectRegister(sim);

        (int, int)[] tests;

        load(sim, 2000, 0, 2);
        tests = new[]
        {
            (1, r.Data[0, 0]),
            (0, r.Byte1),
            (0, r.Byte2),
            (0, r.Byte3),
            (80, r.Data[4, 5]),
        };

        Array.ForEach(
            tests,
            x => Assert.Equal(x.Item1, x.Item2));

        load(sim, 2000, 4, 5);
        tests = new[]
        {
            (0, r.Data[0, 0]),
            (0, r.Byte1),
            (0, r.Byte2),
            (0, r.Byte3),
            (5, r.Data[4, 4]),
            (4, r.Data[5, 5]),
        };

        Array.ForEach(
            tests,
            x => Assert.Equal(x.Item1, x.Item2));
    }

    private void TestRegister(
        Action<Simulator, int, int, int> load,
        Func<Simulator, Register> selectRegister)
    {
        var sim = new Simulator();
        sim.Memory[2000][0, 0] = 1;
        sim.Memory[2000][1, 2] = 80;
        sim.Memory[2000][3, 3] = 3;
        sim.Memory[2000][4, 4] = 5;
        sim.Memory[2000][5, 5] = 4;

        var r = selectRegister(sim);

        load(sim, 2000, 1, 5);
        Assert.Equal(0, r.Data[0, 0]);
        Assert.Equal(80, r.Data[1, 2]);
        Assert.Equal(3, r.Data[3, 3]);
        Assert.Equal(5, r.Data[4, 4]);
        Assert.Equal(4, r.Data[5, 5]);

        load(sim, 2000, 3, 5);
        Assert.Equal(0, r.Data[0, 2]);
        Assert.Equal(3, r.Data[3, 3]);
        Assert.Equal(5, r.Data[4, 4]);
        Assert.Equal(4, r.Data[5, 5]);

        load(sim, 2000, 0, 3);
        Assert.Equal(1, r.Data[0, 0]);
        Assert.Equal(0, r.Data[1, 2]);
        Assert.Equal(80, r.Data[3, 4]);
        Assert.Equal(3, r.Data[5, 5]);

        load(sim, 2000, 4, 4);
        Assert.Equal(0, r.Data[0, 4]);
        Assert.Equal(5, r.Data[5, 5]);

        load(sim, 2000, 0, 0);
        Assert.Equal(1, r.Data[0, 0]);
        Assert.Equal(0, r.Data[1, 5]);
    }
}