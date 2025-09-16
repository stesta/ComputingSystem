using ComputingSystem.Core;

namespace ComputingSystem.Tests;

public class CPUTests
{
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
        int outValue, zr, ng;
        CPU.ALU(zx, nx, zy, ny, f, no, x, y, out outValue, out zr, out ng);

        Assert.Equal(expected, outValue);
    }
}