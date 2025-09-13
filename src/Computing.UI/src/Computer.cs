using Godot;
using System;
using System.Diagnostics;

using ComputingSystem.Components;

public partial class Computer : Node2D
{
    private RAM _ram = new();
    private ROM _rom = new(new int[32678]);
    private CPU _cpu = new();

    [Signal]
    public delegate void ScreenUpdateEventHandler(int[] pixels);

    public void Tick()
    {
        int instruction = _rom.Read(_cpu.PC);
        _cpu.Execute(instruction, _ram);

        EmitSignal("ScreenUpdate", _ram.ReadScreen());
    }
}
