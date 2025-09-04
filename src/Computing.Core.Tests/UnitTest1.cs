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


    [Fact]
    public void Counter_Reset_SetsValueToZero()
    {
        var counter = new Counter();
        counter.Tick(123, 1, 0, 0); // load
        int value = counter.Tick(0, 0, 0, 1); // reset
        Assert.Equal(0, value);
    }

    [Fact]
    public void Counter_Load_SetsValue()
    {
        var counter = new Counter();
        int value = counter.Tick(0x12345, 1, 0, 0); // load
        Assert.Equal(0x2345, value); // only lower 16 bits
    }

    [Fact]
    public void Counter_Inc_IncrementsValue()
    {
        var counter = new Counter();
        counter.Tick(5, 1, 0, 0); // load 5
        int value = counter.Tick(0, 0, 1, 0); // inc
        Assert.Equal(6, value);
    }

    [Fact]
    public void Counter_Inc_WrapsAt16Bits()
    {
        var counter = new Counter();
        counter.Tick(0xFFFF, 1, 0, 0); // load max
        int value = counter.Tick(0, 0, 1, 0); // inc
        Assert.Equal(0, value);
    }

    [Fact]
    public void Counter_Priority_ResetOverLoadAndInc()
    {
        var counter = new Counter();
        counter.Tick(123, 1, 0, 0); // load
        int value = counter.Tick(456, 1, 1, 1); // all signals, reset wins
        Assert.Equal(0, value);
    }

    [Fact]
    public void Counter_Priority_LoadOverInc()
    {
        var counter = new Counter();
        counter.Tick(5, 1, 0, 0); // load
        int value = counter.Tick(10, 1, 1, 0); // load and inc, load wins
        Assert.Equal(10, value);
    }

    [Fact]
    public void RAM_DefaultValueIsZero()
    {
        var ram = new RAM(8);
        for (int i = 0; i < 8; i++)
            Assert.Equal(0, ram.Tick(0, i, 0));
    }

    [Fact]
    public void RAM_WriteAndRead()
    {
        var ram = new RAM(4);
        ram.Tick(123, 2, 1); // write 123 to address 2
        Assert.Equal(123, ram.Tick(0, 2, 0)); // read
    }

    [Fact]
    public void RAM_MasksTo16Bits()
    {
        var ram = new RAM(2);
        ram.Tick(0x12345, 1, 1); // write
        Assert.Equal(0x2345, ram.Tick(0, 1, 0)); // read
    }

    [Fact]
    public void RAM_OutOfRange_Throws()
    {
        var ram = new RAM(2);
        Assert.Throws<ArgumentOutOfRangeException>(() => ram.Tick(0, -1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => ram.Tick(0, 2, 0));
    }

}