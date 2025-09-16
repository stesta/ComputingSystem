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


        var _parser = new Parser(filePath);

        List<string> instructions = new();

        while (_parser.HasMoreLines())
        {
            _parser.Advance();

            var instructionType = _parser.InstructionType();
            if (instructionType == InstructionTypes.A_INSTRUCTION)
            {
                var symbol = _parser.Symbol();
                int symbolValue = int.Parse(symbol);
                
                instructions.Add(Convert.ToString(symbolValue, 2).PadLeft(16, '0'));
            }
            else if (instructionType == InstructionTypes.C_INSTRUCTION)
            {
                var dest = _parser.Dest();
                var comp = _parser.Comp();
                var jump = _parser.Jump();

                instructions.Add($"111{comp}{dest}{jump}");
            }
            else if (instructionType == InstructionTypes.L_INSTRUCTION)
            {
                var symbol = _parser.Symbol();
            }

        }

        File.WriteAllText(filePath.Replace(".asm", ".c.hack"), string.Join(Environment.NewLine, instructions));

        return 0;
    }
}