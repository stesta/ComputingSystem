using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 16 bit register implementation
// support a clock input for edge-level timing
public class Register
{
    private int _dff = 0;
    private int _out = 0;
    private int _nextMux = 0;
    private int _prevClock = 0;

    public void Compute(int input, int load)
    {
        _nextMux = load == 1 ? input & 0xFFFF : _dff;
    }

    // Clock Input Signal Handler
    public void OnClockSignal(int clock)
    {
        if (_prevClock == 0 && clock == 1)
        {
            _dff = _nextMux;
            _out = _nextMux;
        }

        _prevClock = clock;
    }

    public int Out => _out;
}


/// <summary>
/// this is a more general RAM/ROM implementation that can be used for any size
/// it skips the primitive chipsets for efficiency.
/// </summary>
public class RAM
{
    private int[] _registers = new int[32678];

    public int Read(int address) => _registers[address];
    public void Write(int address, int value) => _registers[address] = value;

    public int[] ReadScreen() => _registers[16384..25576];

    public int ReadKeyboard => _registers[24576];
}


/// <summary>
/// this is a more general RAM/ROM implementation that can be used for any size
/// it skips the primitive chipsets for efficiency.
/// </summary>
public class ROM
{
    private int[] _registers; 

    public ROM(int[] registers)
    {
        _registers = registers;
    }

    public int Read(int address) => _registers[address];
}
