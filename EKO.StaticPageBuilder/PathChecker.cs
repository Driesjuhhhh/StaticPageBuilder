using System.Text.Json;

namespace EKO.StaticPageBuilder;

internal static class PathChecker
{
    private const string CONFIG_FILE_NAME = "page_builder.json";

    public static PageBuilderConfig[]? CheckAndConvertArgs(string[] args)
    {
        if (args.Length > 0)
        {
            string jsonFile = string.Empty;

            if (!IsPathValid(args[0]))
            {
                Console.WriteLine("Invalid path for config file.");
                return null;
            }

            string filePath;

            try
            {
                filePath = AddFileIfPathIsDirectory(args[0]);

                if (IsFileUseable(filePath))
                {
                    // File should be valid, read all
                    jsonFile = File.ReadAllText(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                switch (ex)
                {
                    case ArgumentNullException:
                    case ArgumentException:
                        Console.WriteLine("JSON config file path was not provided or was invalid.");
                        break;
                    case FileNotFoundException:
                        Console.WriteLine("JSON config file path was not found.");
                        break;
                    case DirectoryNotFoundException:
                        Console.WriteLine("Directory in given path was not found.");
                        break;
                    case UnauthorizedAccessException:
                        Console.WriteLine("Couldn't access the file. No permission.");
                        break;
                    case PathTooLongException:
                        Console.WriteLine("Input path was too long.");
                        break;
                    default:
                        Console.WriteLine("Unknown error.");
                        break;
                }

                Console.ResetColor();

                return null;
            }

            if (!string.IsNullOrWhiteSpace(jsonFile))
            {
                try
                {
                    var config = JsonSerializer.Deserialize<PageBuilderConfig[]>(jsonFile);

                    if (config is null)
                    {
                        Console.WriteLine("Couldn't parse the JSON file.");
                        return null;
                    }

                    filePath = GetBaseProjectDirectory(filePath);

                    foreach (var item in config)
                    {
                        item.InputPath = Path.Combine(filePath, FixPathName(item.InputPath));
                        item.OutputPath = Path.Combine(filePath, FixPathName(item.OutputPath));
                        item.TemplatePath = Path.Combine(filePath, FixPathName(item.TemplatePath));

                        //if (!string.IsNullOrWhiteSpace(item.ImagesDirectoryPath))
                        //{
                        //    item.ImagesDirectoryPath = Path.Combine(filePath, FixPathName(item.ImagesDirectoryPath));
                        //}

                        if (!string.IsNullOrWhiteSpace(item.WidgetPath))
                        {
                            item.WidgetPath = Path.Combine(filePath, FixPathName(item.WidgetPath));
                        }
                    }

                    return config;
                }
                catch (Exception ex)
                {
                    switch (ex)
                    {
                        case JsonException:
                            Console.WriteLine("JSON config file is invalid.");
                            break;
                        default:
                            Console.WriteLine("Unknown error occurred while deserializing JSON file.");
                            break;
                    }

                    return null;
                }
            }
        }

        return null;
    }

    public static bool IsPathValid(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return false;

        return path.IndexOfAny(Path.GetInvalidPathChars()) == -1;
    }

    private static string GetBaseProjectDirectory(string path)
    {
        return Path.GetDirectoryName(path)!;
    }

    private static string FixPathName(string path)
    {
        return path.Replace("./", "").Replace('/', '\\');
    }

    private static string AddFileIfPathIsDirectory(string path)
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

    private static bool IsFileUseable(string path)
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
