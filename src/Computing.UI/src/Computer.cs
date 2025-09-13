using Godot;
using System;
using System.Diagnostics;

using ComputingSystem.Components;

public partial class Computer : Node2D
{
    private RAM _ram = new();
    private CPU _cpu = new();

    private ROM _rom;

    public Computer()
    {
        // TODO: will eventually load this from an external location
        // could be a file? text input? dynamically compiled hack asm? 
        int[] instructions = [
            0b0000000000000010, // @2
            0b1110110000010000, // D=A
            0b0000000000000011, // @3
            0b1110000010010000, // D=A+D
            0b0000000000000000, // @0
            0b1110001100001000, // M=D
            0b0000000000000110, // @6
            0b1110101010000111, // 0;JMP
        ];

        _rom = new ROM(instructions);
    }

    [Signal]
    public delegate void ScreenUpdateEventHandler(int[] pixels);

    public void Tick()
    {
        int instruction = _rom.Read(_cpu.PC);
        _cpu.Execute(instruction, _ram);

        // TODO: needs some more robust debugging and visualization in UI
        Debug.WriteLine($"A: {_cpu.A}, D: {_cpu.D}, PC: {_cpu.PC}, RAM[0]: {_ram.Read(0)}");

        EmitSignal(SignalName.ScreenUpdate, _ram.ReadScreen());
    }
}
