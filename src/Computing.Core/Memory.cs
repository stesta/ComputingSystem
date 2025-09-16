namespace ComputingSystem.Core;

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


public struct PixelChange
{ 
    public int X;
    public int Y;
    public int Value;
}

/// <summary>
/// this is a more general RAM/ROM implementation that can be used for any size
/// it skips the primitive chipsets for efficiency.
/// </summary>
public class RAM
{
    private int[] _registers = new int[32678];
    

    public int Read(int address) => _registers[address];
    public void Write(int address, int value) 
    {
        value &= 0xFFFF;

        if (address >= 16384 && address <= 25576)
        {
            int index = address - 16384;
            int previousValue = _registers[address];
            
            if (previousValue != value)
            {
                for (int bit = 0; bit < 16; bit++)
                {
                    int previousBit = previousValue >> bit & 1;
                    int newBit = value >> bit & 1;

                    if (previousBit != newBit)
                    {
                        int pixelIndex = index * 16 + bit;
                        _pixelChanges.Add(new()
                        {
                            X = pixelIndex % 512,
                            Y = pixelIndex / 512,
                            Value = newBit
                        });
                    }
                }
            }
        }

        _registers[address] = value;
    }

    private List<PixelChange> _pixelChanges = new();
    public List<PixelChange> ConsumePixelChanges()
    {
        var response = new List<PixelChange>(_pixelChanges);
        _pixelChanges.Clear();

        return response;
    }

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
