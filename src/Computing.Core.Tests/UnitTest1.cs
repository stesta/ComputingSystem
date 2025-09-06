using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.Arm;

namespace Computing.Core.Tests;

public class UnitTest1
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

    [Theory]
    [InlineData(1, 0, 1, 0, 1, 0, 3, 5, 0)] // 0
    [InlineData(1, 1, 1, 1, 1, 1, 3, 5, 1)] // 1
    [InlineData(1, 1, 1, 0, 1, 0, 3, 5, -1)] // -1
    [InlineData(0, 0, 1, 1, 0, 0, 3, 5, 3)] // x
    [InlineData(1, 1, 0, 0, 0, 0, 3, 5, 5)] // y
    [InlineData(0, 0, 1, 1, 0, 1, 3, 5, ~3)] // !x
    [InlineData(1, 1, 0, 0, 0, 1, 3, 5, ~5)] // !y
    [InlineData(0, 0, 1, 1, 1, 1, 3, 5, -3)] // -x
    [InlineData(1, 1, 0, 0, 1, 1, 3, 5, -5)] // -y
    [InlineData(0, 1, 1, 1, 1, 1, 3, 5, 4)] // x+1
    [InlineData(1, 1, 0, 1, 1, 1, 3, 5, 6)] // y+1
    [InlineData(0, 0, 1, 1, 1, 0, 3, 5, 2)] // x-1
    [InlineData(1, 1, 0, 0, 1, 0, 3, 5, 4)] // y-1
    [InlineData(0, 0, 0, 0, 1, 0, 3, 5, 8)] // x+y
    [InlineData(0, 1, 0, 0, 1, 1, 3, 5, -2)] // x-y
    [InlineData(0, 0, 0, 1, 1, 1, 3, 5, 2)] // y-x
    [InlineData(0, 0, 0, 0, 0, 0, 3, 5, 1)] // x&y
    [InlineData(0, 1, 0, 1, 0, 1, 3, 5, 7)] // x|y
    public void ALUTest(int zx, int nx, int zy, int ny, int f, int no, int x, int y, int expected)
    {
        var result = Chips.ALU((zx, nx, zy, ny, f, no), x, y);
        Assert.Equal(expected, result.output);
    }
}