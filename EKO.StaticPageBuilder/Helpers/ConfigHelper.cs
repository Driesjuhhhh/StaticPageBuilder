using EKO.StaticPageBuilder.Helpers.Helpers;
using EKO.StaticPageBuilder.Models;
using System.Text.Json;

namespace EKO.StaticPageBuilder.Helpers;

internal static class ConfigHelper
{
    internal static PageBuilderConfig? ParseConfig(string jsonContent)
    {
        try
        {
            var config = JsonSerializer.Deserialize<PageBuilderConfig>(jsonContent);

            if (config is null)
            {
                LogHelper.LogError("Couldn't parse the JSON file.");
            }

            return config;
        }
        catch (Exception ex)
        {
            string msg = ex switch
            {
                JsonException => "JSON config file is invalid.",
                _ => "Unknown error occurred while deserializing JSON file.",
            };

            LogHelper.LogError(msg);

            throw;
        }
    }

    internal static string LocateConfigFile(string path)
    {
        if (!PathHelper.IsPathValid(path))
        {
            LogHelper.LogError("Invalid path for config file.");
            return string.Empty;
        }

        try
        {
            var filePath = PathHelper.AddFileIfPathIsDirectory(path);

            if (PathHelper.IsFileUseable(filePath))
            {
                return filePath;
            }
            else
            {
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            string msg = ex switch
            {
                ArgumentNullException or ArgumentException => "Config file path was not provided or was invalid.",
                FileNotFoundException => "Config file path was not found.",
                DirectoryNotFoundException => "Directory in given path was not found.",
                UnauthorizedAccessException => "Couldn't access the file. No permission.",
                PathTooLongException => "Input path was too long.",
                _ => "Unknown error.",
            };

            LogHelper.LogError(msg);

            throw;
        }
    }

    internal static void AdjustConfigPaths(PageBuilderConfig config, string path)
    {
        var filePath = PathHelper.GetBaseProjectDirectory(path);

        FixArticlePaths(config.Article, filePath);
        FixPagingPaths(config.Paging, filePath);
        FixHomePaths(config.Home, filePath);
        FixConnectFourPaths(config.ConnectFour, filePath);
    }

    private static void FixConnectFourPaths(ConnectFourConfig config, string path)
    {
        if (config != null)
        {
            FixBasePaths(config, path);

            config.ImagesDirectoryPath = Path.Combine(path, PathHelper.FixPathName(config.ImagesDirectoryPath));
        }
    }

    private static void FixBasePaths(BaseConfig config, string path)
    {
        if (config != null)
        {
            config.InputPath = Path.Combine(path, PathHelper.FixPathName(config.InputPath));
            config.OutputPath = Path.Combine(path, PathHelper.FixPathName(config.OutputPath));
            config.TemplatePath = Path.Combine(path, PathHelper.FixPathName(config.TemplatePath));
        }
    }

    private static void FixArticlePaths(ArticleConfig config, string path)
    {
        if (config != null)
        {
            FixBasePaths(config, path);
        }
    }

    private static void FixPagingPaths(PagingConfig config, string path)
    {
        if (config != null)
        {
            FixBasePaths(config, path);

            config.WidgetPath = Path.Combine(path, PathHelper.FixPathName(config.WidgetPath));
            config.PagingWidgetPath = Path.Combine(path, PathHelper.FixPathName(config.PagingWidgetPath));
        }
    }

    private static void FixHomePaths(HomeConfig config, string path)
    {
        if (config != null)
        {
            FixBasePaths(config, path);

            config.WidgetPath = Path.Combine(path, PathHelper.FixPathName(config.WidgetPath));
        }
    }
}
