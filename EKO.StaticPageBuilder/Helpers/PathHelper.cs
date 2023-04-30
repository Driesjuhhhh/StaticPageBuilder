namespace EKO.StaticPageBuilder.Helpers.Helpers;

/// <summary>
/// Helper methods for working with paths.
/// </summary>
internal static class PathHelper
{
    /// <summary>
    /// Default name used for the config file.
    /// </summary>
    private const string CONFIG_FILE_NAME = "page_builder.json";

    /// <summary>
    /// Checks if the path is valid.
    /// </summary>
    /// <param name="path">Path to check</param>
    /// <returns>True if the path is valid</returns>
    internal static bool IsPathValid(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        return path.IndexOfAny(Path.GetInvalidPathChars()) == -1;
    }

    /// <summary>
    /// Get the directory of a file.
    /// </summary>
    /// <param name="path">Path to use</param>
    /// <returns>Directory path</returns>
    internal static string GetBaseProjectDirectory(string path)
    {
        return Path.GetDirectoryName(path)!;
    }

    /// <summary>
    /// Changes the path to a correct format.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>Changed path</returns>
    internal static string FixPathName(string path)
    {
        return path.Replace("./", "").Replace('/', '\\');
    }

    /// <summary>
    /// Add the default config file name if the path is a directory.
    /// </summary>
    /// <param name="path">Path to check</param>
    /// <returns>Path with the config file</returns>
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

    /// <summary>
    /// Checks if the file is useable.
    /// </summary>
    /// <param name="path">Path to check</param>
    /// <returns>True if the path is an useable config file</returns>
    internal static bool IsFileUseable(string path)
    {
        if (!IsPathValid(path))
            return false;

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
