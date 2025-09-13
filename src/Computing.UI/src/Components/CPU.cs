using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace ComputingSystem.Components;

public class ProgramCounter
{
    private int _value = 0;

    public int Compute(int input, int load, int inc, int reset)
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

    private ProgramCounter _pc = new();

    private bool toggle = false;

    public void Execute(int instruction, RAM M)
    {
        if ((instruction & 0x8000) == 0)
        {
            // A-instruction: 0vvvvvvvvvvvvvvv
            A = instruction & 0x7FFF;
            PC++;
            return;
        }

        // C-instruction: 111accccccdddjjj
        int compBits = (instruction >> 6) & 0x3F;
        int destBits = (instruction >> 3) & 0x07;
        int jumpBits = instruction & 0x07;

        int aluX = D;
        int aluY = (instruction & (1 << 12)) != 0 ? M.Read(A) : A;

        int no = compBits & 0x01;
        int f = (compBits >> 1) & 0x01;
        int ny = (compBits >> 2) & 0x01;
        int zy = (compBits >> 3) & 0x01;
        int nx = (compBits >> 4) & 0x01;
        int zx = (compBits >> 5) & 0x01;

        var (outValue, zr, ng) = ALU((zx, nx, zy, ny, f, no), aluX, aluY);

        // Handle destination bits
        if ((destBits & 0b100) != 0) A = outValue;
        if ((destBits & 0b010) != 0) D = outValue;
        if ((destBits & 0b001) != 0) M.Write(A, outValue);

        // Handle jump bits
        bool jump = ShouldJump(jumpBits, outValue, zr == 1, ng == 1);
        PC = jump ? A : PC + 1;
    }

    private bool ShouldJump(int jumpBits, int outValue, bool zero, bool negative)
    {
        return jumpBits switch
        {
            0b001 => outValue > 0,       // JGT
            0b010 => zero,               // JEQ
            0b011 => outValue >= 0,      // JGE
            0b100 => negative,           // JLT
            0b101 => !zero,              // JNE
            0b110 => outValue <= 0,      // JLE
            0b111 => true,               // JMP
            _ => false
        };
    }

    private static (int output, int zr, int ng) ALU((int zx, int nx, int zy, int ny, int f, int no) controlBits, int x, int y)
    {
        var (zx, nx, zy, ny, f, no) = controlBits;

        // Apply (negation . zero) to x and y
        x = zx == 1 ? 0 : x;
        x = nx == 1 ? ~x : x;
        y = zy == 1 ? 0 : y;
        y = ny == 1 ? ~y : y;

        // Calculate the output
        int output = f == 1 ? x + y : x & y;
        output = no == 1 ? ~output : output;

        // Calculate zr (zero flag) and ng (negative flag)
        int zr = output == 0 ? 1 : 0;
        int ng = output < 0 ? 1 : 0;

        return (output, zr, ng);
    }
}
