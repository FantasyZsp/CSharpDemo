using System.IO;

namespace Common.Resources;

public class ResourcesLoader
{
    public static string Load()
    {
        var resourceStream = typeof(ResourcesLoader).Assembly.GetManifestResourceStream("Common.Resources.test.txt");

        if (resourceStream == null)
        {
            return "null";
        }

        var reader = new StreamReader(resourceStream);
        var contents = reader.ReadToEnd();
        return contents;
    }
}