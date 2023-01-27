namespace Mix.Tests;

using Xunit;

public class InstructionTests
{
    [Fact]
    public void TestDeconstruction()
    {
        var w = new Word()
        {
            [0] = 1,
            [1, 2] = 2000,
            [3] = 4,
            [4] = 5,
            [5] = 8,
        };

        var i = new Instruction(w);

        Assert.Equal(1, i.Sign);
        Assert.Equal(2000, i.AA);
        Assert.Equal(4, i.I);
        Assert.Equal(5, i.F);
        Assert.Equal(8, i.C);
    }

    [Fact]
    public void TestConstruction()
    {
        var i = new Instruction()
        {
            Sign = 1,
            AA = 2000,
            I = 4,
            F = 5,
            C = 8,
        };

        var w = i.Data;
        
        Assert.Equal(1, w[0]);
        Assert.Equal(2000, w[1, 2]);
        Assert.Equal(4, w[3]);
        Assert.Equal(5, w[4]);
        Assert.Equal(8, w[5]);
    }
}