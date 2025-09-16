using ComputingSystem.Compiler;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

var app = new CommandApp<CompileCommand>();
return app.Run(args);

internal sealed class CompileCommand : Command<CompileCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to *.asm file. Defaults to current directory.")]
        [CommandArgument(0, "[filePath]")]
        public string? FilePath { get; init; }

        [CommandOption("-o|--output")]
        public string? OutputPath { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        // get file path of test.asm from current directory
        var filePath = Path.GetFullPath(settings.FilePath ?? "test.asm");

        // check if file exists in current directory
        if (!File.Exists(filePath))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] File '{filePath}'");
            return -1;
        }

        FirstPass(filePath);
        SecondPass(filePath);
        
        return 0;
    }

    private Dictionary<string, int> _symbols = new()
    {
        { "SP", 0 },
        { "LCL", 1 },
        { "ARG", 2 },
        { "THIS", 3 },
        { "THAT", 4 },
        { "R0", 0 },
        { "R1", 1 },
        { "R2", 2 },
        { "R3", 3 },
        { "R4", 4 },
        { "R5", 5 },
        { "R6", 6 },
        { "R7", 7 },
        { "R8", 8 },
        { "R9", 9 },
        { "R10", 10 },
        { "R11", 11 },
        { "R12", 12 },
        { "R13", 13 },
        { "R14", 14 },
        { "R15", 15 },
        { "SCREEN", 16384 },
        { "KBD", 24576 },
    };

    public void FirstPass(string filePath)
    {
        var _parser = new Parser(filePath);
        int newLInstructions = 0;

        while (_parser.HasMoreLines())
        {
            _parser.Advance();
            var instructionType = _parser.InstructionType();
            if (instructionType == InstructionTypes.L_INSTRUCTION)
            {
                var symbol = _parser.Symbol();
                if (!_symbols.ContainsKey(symbol))
                {
                    _symbols.Add(symbol, _parser._currentLineIndex - newLInstructions);
                    newLInstructions++;
                }
            }
        }
    }

    public void SecondPass(string filePath)
    {
        var _parser = new Parser(filePath);

        List<string> instructions = new();

        int newVariables = 0;

        while (_parser.HasMoreLines())
        {
            _parser.Advance();

            var instructionType = _parser.InstructionType();
            if (instructionType == InstructionTypes.A_INSTRUCTION)
            {
                var symbol = _parser.Symbol();
                if (int.TryParse(symbol, out int symbolInt))
                {
                    instructions.Add(Convert.ToString(symbolInt, 2).PadLeft(16, '0'));
                }
                else
                {
                    if (!_symbols.ContainsKey(symbol))
                    {
                        _symbols.Add(symbol, 16 + newVariables);
                        newVariables++;
                    }
                    var address = _symbols[symbol];
                    instructions.Add(Convert.ToString(address, 2).PadLeft(16, '0'));
                }
            }
            else if (instructionType == InstructionTypes.C_INSTRUCTION)
            {
                var dest = _parser.Dest();
                var comp = _parser.Comp();
                var jump = _parser.Jump();

                instructions.Add($"111{comp}{dest}{jump}");
            }
        }

        File.WriteAllText(filePath.Replace(".asm", ".c.hack"), string.Join(Environment.NewLine, instructions));
    }
}