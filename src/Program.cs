namespace UvProjectTools;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UvProject;
using CommandLine;
using CommandLine.Text;

internal class Program
{
//    private static string projectFilename = @"C:\Workspace\BLECODE_Emission\projects\target_apps\prod_test\prod_test\Keil_5\prod_test.uvprojx";
//var projectFilename = "/home/fj/Desktop/BLECODE_Emission/projects/target_apps/prod_test/prod_test/Keil_5/prod_test.uvprojx";

    private static string outputFolder = string.Empty;

    private static void ShowTargets(string projectFilename)
    {
        IncludeFiles includeFiles = new IncludeFiles(projectFilename);        
        Console.WriteLine($"The project {projectFilename} contains the following targets:");
        int i = 1;
        foreach (var target in includeFiles.GetTargets())        
        {
            Console.WriteLine($"{i,3} {target}");
            i++;
        }
    }

    private static void CopyIncludes(string projectFilename, bool doCopy = false)
    {
        IncludeFiles includeFiles = new IncludeFiles(projectFilename);
        foreach (var target in includeFiles.GetTargets())
        {
            CopyIncludesForTarget(target, includeFiles, doCopy);
        }        
    }

    private static  void CopyIncludesForTarget(string target, string projectFilename, bool doCopy = false)
    {
        IncludeFiles includeFiles = new IncludeFiles(projectFilename);        
        CopyIncludesForTarget(target, includeFiles, doCopy);
    }

    private static void CopyIncludesForTarget(string target, IncludeFiles includeFiles, bool doCopy = false)
    {
        Console.WriteLine("Target: " + target);
        var included = includeFiles.GetIncludeFilesForTarget(target);

        foreach (var header in included.Keys)
        {
            HashSet<string> paths = included[header];
            if (paths.Count() == 1) 
            {
                var sourceFile = paths.Single();
                var destFile = Path.Combine(outputFolder, header);
                // Console.WriteLine($"{header} -> {paths.Single()}");
                if (doCopy)
                {
                    Console.WriteLine($"Copying {sourceFile} to {destFile}");
                    File.Copy(sourceFile, destFile);
                }
            }
            else            
            {
                var commonFolder = Utils.FindCommonBase(paths);
                foreach (var path in paths)
                {
                    string destFile = Path.Combine(outputFolder, path.Substring(commonFolder.Length+1));
                    Console.WriteLine($"Copying {path} to {destFile}");
                    var destFolder = Path.GetDirectoryName(destFile);
                    if (destFolder is not null)
                    {
                        Directory.CreateDirectory(destFolder);
                        File.Copy(path, destFile);
                    }
                }
            }
        }
    }

    private static void CopySourcesForTarget(string targetToSelect, string? projectFilename)
    {
        if (projectFilename is null)
        {
            throw new ArgumentException(nameof(projectFilename));
        }

        XmlSerializer serializer = new XmlSerializer(typeof(Project));
        string? projectBase = Path.GetDirectoryName(projectFilename);
        if (projectBase is null)
        {
            throw new Exception($"The projectBase {projectBase} is null");
        }

        using (XmlReader reader = XmlReader.Create(new System.IO.StreamReader(projectFilename)))
        {
            Project? project = serializer.Deserialize(reader) as Project;
            if (project == null)
            {
                Console.WriteLine("Project is null");
                return;
            }

            for (int i = 0; i < project.Targets.TargetList.Count; i++)
            {
                Target? target = project.Targets.TargetList[i];
                if (target?.TargetName == targetToSelect) {
                    foreach (var group in target.Groups.GroupList)
                    {
                        foreach (var file in group.Files.FileList)
                        {
                            if ((file.FilePath is null) || (file.FileName is null))
                            {
                                throw new Exception("An error in the project was found: FilePath or FileName is null");
                            }

                            var sourceFile = Path.Combine(projectBase, file.FilePath);
                            var destFile = Path.Combine(outputFolder, file.FileName);
                            Console.WriteLine($"Copying {sourceFile} to {destFile}");
                            if (File.Exists(destFile)) {
                                Console.WriteLine($"File {destFile} already exists. Skipping");
                            } else {
                                File.Copy(sourceFile, destFile);
                            }
                        }
                    }
                }
            }
        }
    }

    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(Run)
            .WithNotParsed(HandleParseError);
   }

    static void Run(CommandLineOptions options)
    {
        Console.WriteLine($"Target: {options.Target}");
        Console.WriteLine($"Output: {options.Output}");
        Console.WriteLine($"ShowTargets: {options.ShowTargets}");

        if (options.ShowTargets)
        {
            ShowTargets(options.Project);
            Environment.Exit(0);
        }

        outputFolder = options.Output;

        if (string.IsNullOrEmpty(options.Target))
        {
            CopyIncludes(options.Project);
        } 
        else
        { 
            if (options.DoCopy)
            {
                if (string.IsNullOrEmpty(options.Output))
                {
                    Console.WriteLine("If copy is required, then the output folder must be defined.");
                    Environment.Exit(1);
                }

                if (!Directory.Exists(outputFolder))
                {
                    Console.WriteLine($"The output folder {outputFolder} does not exist");
                    Environment.Exit(1);
                }
            }

            CopyIncludesForTarget(options.Target, options.Project, options.DoCopy);
            CopySourcesForTarget(options.Target, options.Project); 
        }
    }

    static void HandleParseError(IEnumerable<Error> errors)
    {
        // Handle parse errors here
    }   
}