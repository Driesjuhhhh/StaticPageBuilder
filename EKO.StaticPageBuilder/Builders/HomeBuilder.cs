using EKO.StaticPageBuilder.Helpers;
using EKO.StaticPageBuilder.Models;
using System.Text;

namespace EKO.StaticPageBuilder.Builders;

internal static class HomeBuilder
{
    internal static void BuildAndSavePages(HomeConfig config, IEnumerable<Page> recents)
    {
        var home = new StringBuilder(FileHelper.ReadFile(config.TemplatePath));

        var widgets = BuildWidgets(config, recents);

        home.Replace("@#WIDGETS#@", widgets);

        FileHelper.WriteFile(config.OutputPath + @"\index.html", home.ToString());
    }

    private static string BuildWidgets(HomeConfig config, IEnumerable<Page> recents)
    {
        var widgets = new StringBuilder();

        foreach (var page in recents)
        {
            var widget = FileHelper.ReadFile(config.WidgetPath);

            widget = widget
                .Replace("@#IMAGESDIRECTORYPATH#@", config.ImagesDirectoryPath)
                .Replace("@#COVER#@", page.PageVars.Find(x => x.Name == "cover")?.Value ?? "code.jpg")
                .Replace("@#TITLE#@", page.PageVars.Find(x => x.Name == "title")?.Value ?? "No title")
                .Replace("@#DESCRIPTION#@", page.PageVars.Find(x => x.Name == "description")?.Value ?? "No description")
                .Replace("@#FILENAME#@", page.MetaData.FileName);

            widgets.Append(widget);

            LogHelper.LogInfo($"Built home widget for {page.MetaData.FileName}.");
        }

        return widgets.ToString();
    }
}