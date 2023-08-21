namespace UvProjectTools;
using CommandLine;
using CommandLine.Text;

internal class CommandLineOptions
{
    [Option("target", Required = false, HelpText = "Specify the target string")]
    public string Target { get; set; } = string.Empty;

    [Option("output", Required = false, HelpText = "Specify the output path")]
    public string Output { get; set; } = string.Empty;

    [Option("show-targets", Required=false, HelpText = "Shows the list of targets contained in the command")]
    public bool ShowTargets {get; set; } = false;

    [Option("project", Required = true, HelpText = "The uVision project to be loaded")]
    public string Project { get; set; } = string.Empty;

    [Option("copy", Required = true, HelpText = "Perform the copy of the files")]
    public bool DoCopy { get; set; } = false;
}