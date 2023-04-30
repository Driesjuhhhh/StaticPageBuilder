using EKO.StaticPageBuilder.Helpers;
using EKO.StaticPageBuilder.Models;

namespace EKO.StaticPageBuilder.Builders;

/// <summary>
/// Build the home / index page.
/// </summary>
internal static class HomeBuilder
{
    /// <summary>
    /// Build the index page.
    /// </summary>
    /// <param name="config">Path to templates and widgets</param>
    /// <param name="recents">List of the most recent articles to show on the home page</param>
    internal static void BuildAndSavePages(HomeConfig config, IEnumerable<Page> recents)
    {
        // Read the template file.
        var home = new StringBuilder(FileHelper.ReadFile(config.TemplatePath));

        // Build the article cards
        var widgets = BuildWidgets(config, recents);

        home.Replace("@#WIDGETS#@", widgets);

        // Save the index page.
        FileHelper.WriteFile(config.OutputPath + @"\index.html", home.ToString());
    }

    /// <summary>
    /// Build the article cards.
    /// </summary>
    /// <param name="config">Path to the widgets</param>
    /// <param name="recents">Most recent pages that will be shown on the page</param>
    /// <returns>Generated HTML</returns>
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