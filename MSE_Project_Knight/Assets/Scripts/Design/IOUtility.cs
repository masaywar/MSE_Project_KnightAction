using System.IO;
using System.Linq;

public static class IOUtility
{
    public static string[] OpenLines(string path)
    {
        path = path.Replace(@"\", @"/");
        return File.
            ReadAllLines(path).
            Select(l => l.Trim()).
            Where(l => !System.String.IsNullOrEmpty(l)).ToArray();
    }
}