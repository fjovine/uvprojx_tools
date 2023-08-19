namespace UvProjectTools;

using System.Xml;
using System.Xml.Serialization;
using UvProject;

internal class Program
{
    private static void Main(string[] args)
    {
        var projectFilename = "/home/fj/Desktop/BLECODE_Emission/projects/target_apps/prod_test/prod_test/Keil_5/prod_test.uvprojx";

        /*
        XmlSerializer serializer = new XmlSerializer(typeof(Project));

        using (XmlReader reader = XmlReader.Create(new System.IO.StreamReader("../UnitTests/prod_test.uvprojx")))
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
                Console.WriteLine("Target Name: " + target.TargetName);
                foreach (var group in target.Groups.GroupList)
                {
                    Console.WriteLine("  Group Name: " + group.GroupName);
                    foreach (var file in group.Files.FileList)
                    {
                        Console.WriteLine("    File Name: " + file.FileName);
                        Console.WriteLine("    File Type: " + file.FileType);
                        Console.WriteLine("    File Path: " + file.FilePath);
                    }
                }
            }
        }
        */

        IncludeFiles includeFiles = new IncludeFiles(projectFilename);
        foreach (var target in includeFiles.GetTargets())
        {
            Console.WriteLine("Target: " + target);
            var included = includeFiles.GetIncludeFilesForTarget(target);

            foreach (var path in included)
            {
                Console.WriteLine($"{path.Key} -> {path.Value}");
            }
        }
    }
}