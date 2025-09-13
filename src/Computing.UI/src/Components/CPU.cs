using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingSystem.Components;

public class ProgramCounter
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

public class CPU
{
    public int A = new();
    public int D = new();
    public int PC = new();

    private bool toggle = false;

    // TODO: need to actually implement the CPU
    public void Execute(int instruction, RAM ram)
    {
        int value = toggle ? 0xFFFF : 0x0000;

        ram.Write(16835, value);
        ram.Write(16836, value);
        ram.Write(16837, value);
        ram.Write(16838, value);

        toggle = !toggle;
    }
}
