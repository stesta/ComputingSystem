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
public class Memory
{
    private int[] _registers = new int[32678];

    // READ
    // WRITE

    //public void Compute(int input, int address, int load)
    //{
    //    if (address < 0 || address >= _registers.Length)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(address), $"Address must be in interval [0,{_registers.Length - 1}].");
    //    }

    //    _registers[address].Compute(input, load);
    //}
}
