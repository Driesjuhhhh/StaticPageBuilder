namespace EKO.StaticPageBuilder.Helpers;

/// <summary>
/// Class for reading and writing files.
/// </summary>
internal static class FileHelper
{
    /// <summary>
    /// Read a file and return its content.
    /// </summary>
    /// <param name="path">Path to read from</param>
    /// <returns>Content of the file that has been read</returns>
    internal static string ReadFile(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            string msg = ex switch
            {
                ArgumentNullException or ArgumentException => "File path was not provided or was invalid.",
                FileNotFoundException => "File path was not found.",
                DirectoryNotFoundException => "Directory in given path was not found.",
                UnauthorizedAccessException => "Couldn't access the file. No permission.",
                PathTooLongException => "Input path was too long.",
                IOException => "File is already being used.",
                _ => "Unknown error.",
            };

            LogHelper.LogError(msg);

            throw;
        }
    }

    /// <summary>
    /// Write a file with the given content.
    /// </summary>
    /// <param name="path">Path to the file to write to</param>
    /// <param name="content">Content to write to the file</param>
    internal static void WriteFile(string path, string content)
    {
        try
        {
            // Check if the directory exists, if not, create it.
            var fileInfo = new FileInfo(path);

            if (!fileInfo.Directory!.Exists)
            {
                fileInfo.Directory.Create();
            }

            File.WriteAllText(path, content);
        }
        catch (Exception ex)
        {
            string msg = ex switch
            {
                ArgumentNullException or ArgumentException => "File path or content was not provided or was invalid.",
                DirectoryNotFoundException => "Directory in given path was not found.",
                UnauthorizedAccessException => "Couldn't access the file. No permission.",
                PathTooLongException => "Input path was too long.",
                _ => "Unknown error.",
            };

            LogHelper.LogError(msg);
        }
    }
}
