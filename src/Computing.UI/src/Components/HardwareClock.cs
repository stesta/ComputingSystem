using Godot;
using System;

public partial class HardwareClock : Timer
{
    [Export(PropertyHint.None, "inital clock value: 0 or 1")]
    public int _clock = 0;

    [Signal]
    public delegate void ClockEventHandler(int clock);

    public HardwareClock()
    {
        Autostart = true;
        Timeout += Clock_Timeout;
        EmitSignal("Clock", _clock);
    }

    private void Clock_Timeout()
    {
        _clock = _clock == 1 ? 0 : 1;
        EmitSignal("Clock", _clock);
    }
}
