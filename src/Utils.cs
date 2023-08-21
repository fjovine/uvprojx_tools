namespace UvProjectTools;
public static class Utils
{
    public static string NormalizeFilenames(string filename)
    {
        return filename.Replace('\\', '/');
    }

    public static string Relative2Absolute(string basepath, string relative)
    {
        string normalizedRelative = NormalizeFilenames(relative);
        string normalizedBasepath = NormalizeFilenames(basepath);
        string combined = Path.Combine(normalizedBasepath, normalizedRelative);
        var result = Path.GetFullPath(combined);
        return result;
    }

    public static string FindCommonStart(IEnumerable<string> strs)
    {
        string commonStart = string.Empty;
        bool isStart = true;
        foreach (var str in strs) {
            if (isStart) {
                commonStart = str;
                isStart = false;
            } else {
                commonStart = CommonStart(str, commonStart, out int _);
            }
        }

        return commonStart;
    }

    public static string CommonStart(string s1, string s2, out int maxCommonIndex)
    {
        maxCommonIndex = 0;
        for (maxCommonIndex=0; maxCommonIndex < Math.Min(s1.Length, s2.Length); maxCommonIndex++)
        {

            if (s1[maxCommonIndex] != s2[maxCommonIndex]) {
                break;
            }
        }

        if (maxCommonIndex == 0) {
            return string.Empty;
        } else {
            return s1.Substring(0,maxCommonIndex);
        }
    }

    public static string FindCommonBase(IEnumerable<string> paths)
    {
        string result = string.Empty;

        bool isFirst = true;
        foreach (var path in paths)
        {
            if (isFirst) {
                result = path;
                isFirst = false;
            } else {
                result = FindCommonPath(result, path);
            }
        }

        return result;
    }

    public static string FindCommonPath(string path1, string path2)
    {
        string[] folders1 = path1.Split(Path.DirectorySeparatorChar);
        string[] folders2 = path2.Split(Path.DirectorySeparatorChar);
        string result = string.Empty;
        for (var index =0; index< Math.Min(folders1.Length, folders2.Length); index++) 
        {
            if (folders1[index] != folders2[index]) 
            {
                break;
            }
            result = Path.Combine(result, folders1[index]);
        }

        return result;
    }
}