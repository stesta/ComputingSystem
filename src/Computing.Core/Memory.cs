using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computing.Core;

// TODO: timing on this is probably wrong, need to check against the clock
public class PC
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


/// <summary>
/// this is a more general RAM/ROM implementation that can be used for any size
/// it skips the primitive chipsets for efficiency
/// </summary>
public class Memory
{
    private int[] _registers;

    public Memory(int size = 16834)
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

