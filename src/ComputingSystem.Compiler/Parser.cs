using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingSystem.Compiler
{
    internal enum InstructionTypes
    {
        A_INSTRUCTION,
        C_INSTRUCTION,
        L_INSTRUCTION
    }

    internal sealed class Parser
    {
        private string[] _lines;
        private int _totalLines;
        private int _currentLineIndex = -1;

        private Dictionary<string, int> _symbols = new();

        public Parser(string filePath)
        {
            _lines = File.ReadAllLines(filePath);
            _totalLines = _lines.Length;
        }

        public bool HasMoreLines() => _currentLineIndex + 1 < _totalLines;

        public void Advance()
        {
            _currentLineIndex++;
            
            var line = _lines[_currentLineIndex].Trim();
            if (line.Length == 0 || line.StartsWith("//"))
            {
                Advance();
            }
        }

        public InstructionTypes InstructionType()
        {
            var instruction = _lines[_currentLineIndex].Trim();

            return instruction switch 
            {
                _ when instruction.StartsWith("@") => InstructionTypes.A_INSTRUCTION,
                _ when instruction.StartsWith("(") && instruction.EndsWith(")") => InstructionTypes.L_INSTRUCTION,
                _ => InstructionTypes.C_INSTRUCTION
            };
        }

        public string Symbol()
        {
            switch (InstructionType())
            {
                case InstructionTypes.A_INSTRUCTION:
                    var aInstruction = _lines[_currentLineIndex].Trim();
                    return aInstruction[1..]; // Skip the '@' character
                case InstructionTypes.L_INSTRUCTION:
                    var lInstruction = _lines[_currentLineIndex].Trim();
                    return lInstruction[1..^1]; // Skip the '(' and ')' characters
                default:
                    throw new Exception("Symbol() called on non A- or L-instruction");
            }
        }

        public string Dest()
        {
            if (InstructionType() != InstructionTypes.C_INSTRUCTION)
                throw new Exception("Dest() called on non-C-instruction");

            if (!_lines[_currentLineIndex].Contains('='))
                return "000";

            var instruction = _lines[_currentLineIndex].Trim();
            var destination = instruction.Split('=')[0];

            return Code.Dest(destination);
        }

        public string Comp()
        {
            if (InstructionType() != InstructionTypes.C_INSTRUCTION)
                throw new Exception("Dest() called on non-C-instruction");


            var instruction = _lines[_currentLineIndex].Trim();
            var destination = instruction.Contains("=") 
                ? instruction.Split('=')[1]
                : instruction.Split(';')[0];

            return Code.Comp(destination);
        }

        public string Jump()
        {
            if (InstructionType() != InstructionTypes.C_INSTRUCTION)
                throw new Exception("Dest() called on non-C-instruction");

            if (!_lines[_currentLineIndex].Contains(';'))
                return "000";

            var instruction = _lines[_currentLineIndex].Trim();
            var jump = instruction.Split(';')[1];

            return Code.Jump(jump);
        }   
    }
}
