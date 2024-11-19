using CommandLine;

namespace Conversion.Configuration;

public class CommandLineOptions
{
    [Option('f', "file", Required = true, HelpText = "Full path to the Markdown file.")]
    public required string MarkdownFilePath { get; init; }
    
    [Option('d', "output-dir-name", Required = false, HelpText = "Directory name for the converted HTML file.", Default = "Desktop directory")]
    public required string OutputDirectoryName { get; init; }
    
    [Option('o', "open-on-completion", Required = false, HelpText = "Opens the created HTML file with the default application for HTML files.")]
    public bool OpenOnCompletion { get; init; }
    
    [Option("open-with-browser", Required = false, HelpText = "Forces opening with Browser.")]
    public bool OpenWithBrowser { get; init; }
}