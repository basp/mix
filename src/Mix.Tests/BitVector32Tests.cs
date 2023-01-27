namespace Mix.Tests;

using Xunit;

public class BitVector32Tests
{
    [Fact]
    public void BitVector32Construction()
    {
        var v = new BitVector32();
        Assert.Equal(0, v.Data);

        var s0 = BitVector32.CreateSection(3, 2);
        v[s0] = 5;

        Assert.Equal(5, v[s0]);

        var s1 = BitVector32.CreateSection(1, 0);
        v[s1] = 1;
        Assert.Equal(1, v[s1]);

        v[s1] = 0;
        Assert.Equal(0, v[s1]);

        var s2 = BitVector32.CreateSection(7, 1);
        v[s2] = 127;
        Assert.Equal(127, v[s2]);
    }
}