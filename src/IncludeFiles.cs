using System.Xml;
namespace UvProjectTools;

public class IncludeFiles
{
    private Dictionary<string, HashSet<string>> target2IncludedPaths = new();

    private string basePath = string.Empty;

    static string GetTarget(XmlNode node)
    {
        if (node is null)
        {
            throw new System.ArgumentNullException(nameof(node));
        }

        string result = string.Empty;
        XmlNode? parent = node.ParentNode;

        while (parent != null)
        {
            if (parent.Name == "Target")
            {
                XmlNode? targetNode = parent.SelectSingleNode("TargetName");
                if (targetNode != null)
                {
                    result = targetNode.InnerText;
                }
                break;
            }

            parent = parent.ParentNode;
        }

        return result;
    }

    public IncludeFiles(string projectPath)
    {
        if (!Path.IsPathRooted(projectPath))
        {
            throw new ArgumentException("projectPath must be an absolute path");
        }

        basePath = Path.GetDirectoryName(projectPath)!;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(projectPath); // Replace with the actual path
        XmlNodeList? includePathNodes = xmlDoc.SelectNodes("//IncludePath");

        if (includePathNodes is null)
        {
            throw new Exception("No Includepath in project");
        }

        foreach (XmlNode includePathNode in includePathNodes)
        {
            string includePath = includePathNode.InnerText.Trim();
            string[] includePaths = includePath.Split(';');
            if (includePaths.Length > 1)
            {
                var target = GetTarget(includePathNode);
                if (string.IsNullOrEmpty(target))
                {
                    continue;
                }

                if (!target2IncludedPaths.ContainsKey(target))
                {
                    target2IncludedPaths[target] = new HashSet<string>();
                }

                foreach (var path in includePaths)
                {
                    if (Path.IsPathRooted(path))
                    {
                        target2IncludedPaths[target].Add(path);
                    }
                    else
                    {
                        target2IncludedPaths[target].Add(Utils.Relative2Absolute(basePath, path));
                    }
                }
            }
        }
    }

    public List<string> GetIncludePaths(string target)
    {
        var result = new List<string>();
        if (target2IncludedPaths.ContainsKey(target))
        {
            result = target2IncludedPaths[target].ToList();
        }

        return result;
    }

    public List<string> GetTargets()
    {
        return target2IncludedPaths.Keys.ToList();
    }

    /// <summary>
    /// This method builds a map between an include filename (*.h) and a set of paths where this headers are found.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public Dictionary<string, HashSet<string>> GetIncludeFilesForTarget(string target)
    {
        if (!ContainsTarget(target))
        {
            throw new ArgumentException("Target not found");
        }

        var result = new Dictionary<string, HashSet<string>>();

        foreach (var path in GetIncludePaths(target))
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"The path {path} does not exist");
                continue;
            }
            foreach (var headerPath in Directory.GetFiles(path, "*.h", SearchOption.AllDirectories))
            {
                var headerFilename = Path.GetFileName(headerPath);
                
                if (!result.ContainsKey(headerFilename)) {
                    result[headerFilename] = new HashSet<string>();
                }

                result[headerFilename].Add(headerPath);
            }
        }

        return result;
    }

    public bool ContainsTarget(string target)
    {
        return target2IncludedPaths.ContainsKey(target);
    }
}