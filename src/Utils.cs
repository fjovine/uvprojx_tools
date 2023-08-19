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
}