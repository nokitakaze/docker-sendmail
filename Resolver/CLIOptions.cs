using CommandLine;

namespace Resolver;

// ReSharper disable once ClassNeverInstantiated.Global
public class CLIOptions
{
    [Option('i', "input", Required = true, HelpText = "File with domains list")]
    public string InputFilename { get; set; } = string.Empty;

    [Option('o', "output", Required = true, HelpText = "File with output file")]
    public string OutputFilename { get; set; } = string.Empty;
}