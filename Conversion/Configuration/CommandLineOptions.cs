using CommandLine;

namespace Conversion.Configuration;

public class CommandLineOptions
{
    [Option('f', "file", Required = true, HelpText = "Full or relative path to the Markdown file.")]
    public required string MarkdownFilePath { get; init; }
    
    [Option('o', "output-dir-name", Required = false, HelpText = "Directory name for the converted HTML file.", Default = ".")]
    public required string OutputDirectoryName { get; init; }
    
    [Option("open", Required = false, HelpText = "Opens the created HTML file with the default application for HTML files.")]
    public bool OpenOnCompletion { get; init; }
    
    [Option("open-with-browser", Required = false, HelpText = "Forces opening with Browser.")]
    public bool OpenWithBrowser { get; init; }
    
    [Option('v', "verbose", Max = 3, FlagCounter = true, HelpText = "Verbosity of logs, v, vv, or vvv")]
    public int Verbosity { get; init; }
    
}
