using Godot;
using ComputingSystem.Core;
using ComputingSystem.Peripherals;
using System.Diagnostics;
using System.Threading;

namespace ComputingSystem;

public partial class Computer : Node2D
{
    private RAM _ram = new();
    private CPU _cpu = new();
    private ROM _rom;

    [Export]
    public Screen Screen { get; set; }

    // Clock speed of the emulated CPU. This is intentionally decoupled from
    // the render frame rate: the CPU runs on its own thread as fast as this
    // allows, and the UI thread just paints whatever pixels changed since
    // the last rendered frame.
    [Export]
    public int TicksPerSecond { get; set; } = 1_000_000; // 1MHz

    private double _tickInterval => 1.0 / TicksPerSecond;

    private Thread _cpuThread;
    private volatile bool _running;

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

    public override void _Ready()
    {
        _running = true;
        _cpuThread = new Thread(RunCpuLoop) { IsBackground = true };
        _cpuThread.Start();
    }

    public override void _ExitTree()
    {
        _running = false;
        _cpuThread?.Join();
    }

    public override void _Process(double delta)
    {
        var pixels = _ram.ConsumePixelChanges();
        if (pixels.Count > 0)
        {
            Screen.SetPixels(pixels);
        }
    }

    // Runs on a background thread, independent of the render frame rate, so
    // the emulated computer keeps executing instructions even if a frame
    // takes a while to render. Paced against a Stopwatch rather than tied
    // to any single frame's delta.
    private void RunCpuLoop()
    {
        var stopwatch = Stopwatch.StartNew();
        double lastElapsed = 0;
        double accumulatedTime = 0;

        while (_running)
        {
            double elapsed = stopwatch.Elapsed.TotalSeconds;
            accumulatedTime += elapsed - lastElapsed;
            lastElapsed = elapsed;

            bool ticked = false;
            while (accumulatedTime >= _tickInterval)
            {
                Tick();
                accumulatedTime -= _tickInterval;
                ticked = true;
            }

            if (!ticked)
            {
                Thread.Sleep(1);
            }
        }
    }

    public void Tick()
    {
        int instruction = _rom.Read(_cpu.PC);
        _cpu.Execute(instruction, _ram);

        // TODO: needs some more robust debugging and visualization in UI
        //Debug.WriteLine($"A: {_cpu.A}, D: {_cpu.D}, PC: {_cpu.PC}, RAM[0]: {_ram.Read(0)}");
    }
}
