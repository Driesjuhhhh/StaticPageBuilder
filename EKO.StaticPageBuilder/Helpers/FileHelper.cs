using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKO.StaticPageBuilder.Helpers;

internal static class FileHelper
{
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
                _ => "Unknown error.",
            };

            LogHelper.LogError(msg);

            throw;
        }
    }

    internal static void WriteFile(string path, string content)
    {
        try
        {
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
