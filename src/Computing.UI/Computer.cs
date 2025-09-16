using Godot;
using ComputingSystem.Core;
using ComputingSystem.Peripherals;

namespace ComputingSystem;

public partial class Computer : Node2D
{
    private RAM _ram = new();
    private CPU _cpu = new();
    private ROM _rom;

    [Export]
    public Screen Screen { get; set; }

    [Export]
    public int TicksPerSecond { get; set; } = 2500;

    private float _tickInterval => 1.0f / TicksPerSecond;

    public Computer()
    {
        // TODO: will eventually load this from an external location
        // could be a file? text input? dynamically compiled hack asm? 
        //int[] instructions = [
        //    0b0000000000000010, // @2
        //    0b1110110000010000, // D=A
        //    0b0000000000000011, // @3
        //    0b1110000010010000, // D=A+D
        //    0b0000000000000000, // @0
        //    0b1110001100001000, // M=D
        //    0b0000000000000110, // @6
        //    0b1110101010000111, // 0;JMP
        //];

        int[] instructions = new int[]
        {
            0b0000000000010000,
            0b1110110000010000,
            0b0000000000010111,
            0b1110001100000110,
            0b0000000000010000,
            0b1110001100001000,
            0b0100000000000000,
            0b1110110000010000,
            0b0000000000010001,
            0b1110001100001000,
            0b0000000000010001,
            0b1111110000100000,
            0b1110111010001000,
            0b0000000000010001,
            0b1111110000010000,
            0b0000000000100000,
            0b1110000010010000,
            0b0000000000010001,
            0b1110001100001000,
            0b0000000000010000,
            0b1111110010011000,
            0b0000000000001010,
            0b1110001100000001,
            0b0000000000010111,
            0b1110101010000111,
        };

        _rom = new ROM(instructions);
    }

    private double _accumulatedTime = 0;

    public override void _Process(double delta)
    {
        _accumulatedTime += delta;
        while (_accumulatedTime >= _tickInterval)
        {
            Tick();
            _accumulatedTime -= _tickInterval;
        }
    }


    public void Tick()
    {
        int instruction = _rom.Read(_cpu.PC);
        _cpu.Execute(instruction, _ram);

        // TODO: needs some more robust debugging and visualization in UI
        //Debug.WriteLine($"A: {_cpu.A}, D: {_cpu.D}, PC: {_cpu.PC}, RAM[0]: {_ram.Read(0)}");

        var pixels = _ram.ConsumePixelChanges();
        Screen.SetPixels(pixels);
    }

    
}
