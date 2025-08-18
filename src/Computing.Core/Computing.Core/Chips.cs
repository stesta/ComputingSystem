namespace Computing.Core;

public static class Chips
{
    /// <summary>
    /// Nand implemented as a bitwise operation because this is considered a primitive.
    /// </summary>
    public static int Nand(int a, int b)
    {
        return ~(a & b);
    }

    public static int Not(int a)
    {
        return Nand(a, a);
    }

    public static int And(int a, int b)
    {
        return Not(Nand(a, b));
    }

    public static int Or(int a, int b)
    {
        return Nand(Not(a), Not(b));
    }

    public static int Xor(int a, int b)
    {
        return And(Or(a, b), Nand(a, b));
    }

    public static int Mux_MBit(int load, int a, int b)
    {
        load = load == 0 ? 0 : -1;
        return (a & ~load) | (b & load);
    }

    public static int Mux_4way_MBit(int load0, int load1, int a, int b, int c, int d)
    {
        return Mux_MBit(load0, Mux_MBit(load1, a, b), Mux_MBit(load1, c, d));
    }

    public static int Mux_8way_MBit(int load0, int load1, int load2, int a, int b, int c, int d, int e, int f, int g, int h)
    {
        return Mux_MBit(load0, Mux_4way_MBit(load1, load2, a, b, c, d), Mux_4way_MBit(load1, load2, e, f, g, h));
    }

    public static (int, int) DMux_MBit(int load, int a)
    {
        load = load == 0 ? 0 : -1;
        return (a & ~load, a & load);
    }

    public static (int, int, int, int) DMux_4way_MBit(int load0, int load1, int a)
    {
        var (a0, a1) = DMux_MBit(load0, a);
        var (b0, b1) = DMux_MBit(load1, a1);
        return (a0, b0, b1, a1);
    }

    ///// <summary>
    ///// Simulates a simple ALU (Arithmetic Logic Unit) based on the Hack ALU API.
    ///// For efficiency, this ignores the primitive chipset and directly implements the ALU logic.
    ///// </summary>
    ///// <param name="controlBits">
    ///// A tuple of six control bits:
    ///// zx - if 1, zero the x input;
    ///// nx - if 1, negate the x input;
    ///// zy - if 1, zero the y input;
    ///// ny - if 1, negate the y input;
    ///// f  - if 1, compute x + y; if 0, compute x & y;
    ///// no - if 1, negate the output.
    ///// </param>
    ///// <param name="x">First input value.</param>
    ///// <param name="y">Second input value.</param>
    ///// <returns>
    ///// A tuple containing:
    ///// output - the result of the ALU operation;
    ///// zr - 1 if output is zero, otherwise 0;
    ///// ng - 1 if output is negative, otherwise 0.
    ///// </returns>
    public static (int output, int zr, int ng) ALU((int zx, int nx, int zy, int ny, int f, int no) controlBits, int x, int y)
    {
        var (zx, nx, zy, ny, f, no) = controlBits;

        // Apply (negation . zero) to x and y
        x = zx == 1 ? 0 : x;
        x = nx == 1 ? ~x : x;
        y = zy == 1 ? 0 : y;
        y = ny == 1 ? ~y : y;

        // Calculate the output
        int output = f == 1 ? x + y : x & y;
        output = no == 1 ? ~output : output;

        // Calculate zr (zero flag) and ng (negative flag)
        int zr = output == 0 ? 1 : 0;
        int ng = output < 0 ? 1 : 0;

        return (output, zr, ng);
    }

    public static IEnumerable<int> Clock()
    {
        while (true)
        {
            yield return 0;
            yield return 1;
        }
    }
}


public class Counter
{
    private int _value = 0;

    public int Tick(int input, int load, int inc, int reset)
    {
        if (reset != 0)
        {
            _value = 0;
        }
        else if (load != 0)
        {
            _value = input & 0xFFFF; // Ensure we only keep the lower 16 bits
        }
        else if (inc != 0)
        {
            _value = (_value + 1) & 0xFFFF; // Increment and wrap around at 16 bits
        }

        return _value;
    }
}


public class RAM
{
    private int[] _registers;

    public RAM(int size = 16834)
    {
        _registers = new int[size];
        for (int i = 0; i < size; i++)
        {
            _registers[i] = new();
        }
    }

    public int Tick(int input, int address, int load)
    {
        if (address < 0 || address >= _registers.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(address), $"Address must be in interval [0,{_registers.Length - 1}].");
        }

        if (load != 0)
        {
            _registers[address] = input & 0xFFFF; // Ensure we only keep the lower 16 bits
        }

        return _registers[address];
    }
}


// 16 bit register implementation
public class Register
{
    private int _value = 0;

    public int Tick(int input, int load)
    {
        if (load != 0)
        {
            _value = input & 0xFFFF; // Ensure we only keep the lower 16 bits
        }

        return _value;
    }
}

public class RAM8
{
    private Register[] _registers = [new(), new(), new(), new(), new(), new(), new(), new()];

    public int Tick(int input, int address, int load)
    {
        if (address < 0 || address >= _registers.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(address), "Address must be between 0 and 7.");
        }

        return _registers[address].Tick(input, load);
    }
}


public class RAM64
{
    private RAM8[] _ram8s = [new(), new(), new(), new(), new(), new(), new(), new()];

    public int Tick(int input, int address, int load)
    {
        if (address < 0 || address >= _ram8s.Length * 8)
        {
            throw new ArgumentOutOfRangeException(nameof(address), "Address must be between 0 and 63.");
        }

        int ram8Index = address / 8;
        int ram8Address = address % 8;

        return _ram8s[ram8Index].Tick(input, ram8Address, load);
    }
}

public class RAM512
{
    private RAM64[] _ram64s = [new(), new(), new(), new(), new(), new(), new(), new()];

    public int Tick(int input, int address, int load)
    {
        if (address < 0 || address >= _ram64s.Length * 64)
        {
            throw new ArgumentOutOfRangeException(nameof(address), "Address must be between 0 and 511.");
        }
        int ram64Index = address / 64;
        int ram64Address = address % 64;
        return _ram64s[ram64Index].Tick(input, ram64Address, load);
    }
}

public class RAM4K
{
    private RAM512[] _ram512s = [new(), new(), new(), new(), new(), new(), new(), new()];

    public int Tick(int input, int address, int load)
    {
        if (address < 0 || address >= _ram512s.Length * 512)
        {
            throw new ArgumentOutOfRangeException(nameof(address), "Address must be between 0 and 4095.");
        }
        int ram512Index = address / 512;
        int ram512Address = address % 512;
        return _ram512s[ram512Index].Tick(input, ram512Address, load);
    }
}

public class RAM16K
{
    private RAM4K[] _ram4Ks = [new(), new(), new(), new()];

    public int Tick(int input, int address, int load)
    {
        if (address < 0 || address >= _ram4Ks.Length * 4096)
        {
            throw new ArgumentOutOfRangeException(nameof(address), "Address must be in closed interval [0,16383]");
        }
        int ram4KIndex = address / 4096;
        int ram4KAddress = address % 4096;
        return _ram4Ks[ram4KIndex].Tick(input, ram4KAddress, load);
    }
}
