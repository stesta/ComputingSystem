using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ComputingSystem.Core;
using System;

public class CpuBenchmarks
{
    private CPU cpu;
    private RAM ram;
    private int[] instructions;

    [GlobalSetup]
    public void Setup()
    {
        cpu = new CPU();
        ram = new RAM();
        // Example: fill with random instructions (simulate workload)
        var rand = new Random(42);
        instructions = [
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
        ];
        for (int i = 0; i < instructions.Length; i++)
        {
            // Use random 16-bit values as instructions
            instructions[i] = 0b1110101010000111;
        }
    }

    [Benchmark(Description = "rect.asm")]
    public void RunRectagleProgramAndReadScreenChanges()
    {
        for (int pc = 0; pc < instructions.Length; pc++)
        {
            var instruction = instructions[pc];
            cpu.Execute(instruction, ram);
            var pixels = ram.ConsumePixelChanges();
        }   
    }
}

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<CpuBenchmarks>();
        }
    }
