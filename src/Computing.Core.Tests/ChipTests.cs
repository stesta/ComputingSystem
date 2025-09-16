using ComputingSystem.Core;

namespace ComputingSystem.Tests;

public class ChipTests
{
    private const int _0 = unchecked((int)0b00000000000000000000000000000000);
    private const int _1 = unchecked((int)0b11111111111111111111111111111111);


    [Theory]
    [InlineData(_0, _0, _1)]
    [InlineData(_0, _1, _1)]
    [InlineData(_1, _0, _1)]
    [InlineData(_1, _1, _0)]
    public void NandChipTest(int a, int b, int expected)
    {
        var result = Chips.Nand(a, b);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(_0, _1)]
    [InlineData(_1, _0)]
    public void NotChipTest(int a, int expected)
    {
        var result = Chips.Not(a);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(_0, _0, _0)]
    [InlineData(_0, _1, _0)]
    [InlineData(_1, _0, _0)]
    [InlineData(_1, _1, _1)]
    public void AndChipTest(int a, int b, int expected)
    {
        var result = Chips.And(a, b);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(_0, _0, _0)]
    [InlineData(_0, _1, _1)]
    [InlineData(_1, _0, _1)]
    [InlineData(_1, _1, _1)]
    public void OrChipTest(int a, int b, int expected)
    {
        var result = Chips.Or(a, b);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(_0, _0, _0)]
    [InlineData(_0, _1, _1)]
    [InlineData(_1, _0, _1)]
    [InlineData(_1, _1, _0)]
    public void XorChipTest(int a, int b, int expected)
    {
        var result = Chips.Xor(a, b);
        Assert.Equal(expected, result);
    }
}