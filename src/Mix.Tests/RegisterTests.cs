namespace Mix.Tests;

using System;
using Xunit;

public class RegisterTests
{
    [Fact]
    public void TestRegisterConstruction()
    {
        var r = new Register("r");
        Assert.Equal(0, r.Sign);
        Assert.Equal(0, r.Value);
    }

    [Fact]
    public void TestRegisterMaxValue()
    {
        const int maxRegisterValue = (1 << (5 * 6)) - 1;
        const int maxByteValue = (1 << 6) - 1;

        var r = new Register("r")
        {
            Value = -maxRegisterValue
        };

        var bytes = new[]
        {
            r.Byte1,
            r.Byte2,
            r.Byte3,
            r.Byte4,
            r.Byte5,
        };

        Assert.Equal(1, r.Sign);
        Assert.Equal(-maxRegisterValue, r.Value);
        Array.ForEach(bytes, @byte => Assert.Equal(maxByteValue, @byte));
    }

    [Fact]
    public void TestIndexRegisterMaxValue()
    {
        const int maxRegisterValue = (1 << (2 * 6)) - 1;
        const int maxByteValue = (1 << 6) - 1;

        var r = new IndexRegister("r")
        {
            Value = -maxRegisterValue,
        };

        var tests = new[]
        {
            (0, r.Byte1),
            (0, r.Byte2),
            (0, r.Byte3),
            (maxByteValue, r.Byte4),
            (maxByteValue, r.Byte5),
        };

        foreach (var (expected, actual) in tests)
        {
            Assert.Equal(expected, actual);
        }
    }
}