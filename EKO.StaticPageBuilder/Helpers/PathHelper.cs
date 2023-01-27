namespace EKO.StaticPageBuilder.Helpers.Helpers;

internal static class PathHelper
{
    private const string CONFIG_FILE_NAME = "page_builder.json";

    internal static bool IsPathValid(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return false;

        return path.IndexOfAny(Path.GetInvalidPathChars()) == -1;
    }

    internal static string GetBaseProjectDirectory(string path)
    {
        return Path.GetDirectoryName(path)!;
    }

    internal static string FixPathName(string path)
    {
        return path.Replace("./", "").Replace('/', '\\');
    }

    internal static string AddFileIfPathIsDirectory(string path)
    {
        // Check if the user input path is a directory or a file
        var attributes = File.GetAttributes(path);

        if (attributes == FileAttributes.Directory)
        {
            // Try to find the config file
            path = Path.Combine(path, CONFIG_FILE_NAME);
        }

        return path;
    }

    internal static bool IsFileUseable(string path)
    {
        if (!IsPathValid(path)) return false;

        // Get the file name
        var fileName = Path.GetFileName(path);

        // Check if the file has the correct name
        if (fileName.Trim().ToLower() != CONFIG_FILE_NAME)
        {
            Console.WriteLine("Couldn't find the required 'page_builder.json' file.");
            return false;
        }

        return true;
    }
}
