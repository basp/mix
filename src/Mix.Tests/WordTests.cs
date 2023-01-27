namespace Mix.Tests;

using Xunit;

public class WordTests
{
    [Fact]
    public void WordConstruction()
    {
        var word = new Word();
        Assert.Equal(0, word.Value);
    }

    [Fact]
    public void FieldIndexing()
    {
        var word = new Word();
        var values = new [] { 0, 50, 60, 61, 62, 63 };
        
        for (var i = 0; i < values.Length; i++)
        {
            word[i] = values[i];
        }

        for (var i = 0; i < values.Length; i++)
        {
            Assert.Equal(values[i], word[i]);
        }
    }

    [Fact]
    public void FieldSpecIndexing()
    {
        var word = new Word();

        word[0, 0] = (1 << (1 * 1)) - 1;
        word[1, 2] = (1 << (2 * 6)) - 1;
        word[3, 5] = (1 << (3 * 6)) - 1;

        Assert.Equal((1 << (1 * 1)) - 1, word[0, 0]);
        Assert.Equal((1 << (2 * 6)) - 1, word[1, 2]);
        Assert.Equal((1 << (3 * 6)) - 1, word[3, 5]);
    }
}