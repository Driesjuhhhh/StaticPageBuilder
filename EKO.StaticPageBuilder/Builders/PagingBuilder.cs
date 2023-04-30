using EKO.StaticPageBuilder.Helpers;
using EKO.StaticPageBuilder.Models;

namespace EKO.StaticPageBuilder.Builders;

/// <summary>
/// Build the pages for the paging.
/// </summary>
internal static class PagingBuilder
{
    /// <summary>
    /// Builds the pages for the paging.
    /// </summary>
    /// <param name="config">Paths to the pages</param>
    /// <param name="pagedArticles">Paged list of pages</param>
    internal static void BuildAndSavePages(PagingConfig config, List<IEnumerable<Page>> pagedArticles)
    {
        for (int i = 0; i < pagedArticles.Count; i++)
        {
            var builder = new StringBuilder(FileHelper.ReadFile(config.TemplatePath));

            // Add the article cards to the page
            builder.Replace("@#WIDGETS#@", BuildWidgets(config, pagedArticles[i]));
            LogHelper.LogSuccess("Added widgets to the page");

            // Add the paging widget to the page
            builder.Replace("@#PAGING#@", BuildPaging(config, pagedArticles, i));
            LogHelper.LogSuccess("Added paging widget to the page.");

            // Save the page.
            FileHelper.WriteFile(config.OutputPath + $"{i + 1}/index.html", builder.ToString());
        }
    }

    /// <summary>
    /// Builds the cards that get shown on the page
    /// </summary>
    /// <param name="config">Path to widgets</param>
    /// <param name="articles">Articles for the current page</param>
    /// <returns>Generated HTML</returns>
    private static string BuildWidgets(PagingConfig config, IEnumerable<Page> articles)
    {
        var widgetList = new StringBuilder();

        foreach (var page in articles)
        {
            // Read the widget template
            var widget = FileHelper.ReadFile(config.WidgetPath);

            // Build the card widget
            widget = widget
                        .Replace("@#IMAGESDIRECTORYPATH#@", config.ImagesDirectoryPath)
                        .Replace("@#COVER#@", page.PageVars.Find(x => x.Name == "cover")?.Value ?? "code.jpg")
                        .Replace("@#TITLE#@", page.PageVars.Find(x => x.Name == "title")?.Value ?? "No title")
                        .Replace("@#DESCRIPTION#@", page.PageVars.Find(x => x.Name == "description")?.Value ?? "No description")
                        .Replace("@#FILENAME#@", page.MetaData.FileName);

            widgetList.Append(widget);

            LogHelper.LogInfo($"Built widget for {page.MetaData.FileName}.");
        }

        return widgetList.ToString();
    }

    /// <summary>
    /// Builds the paging widget for the page
    /// </summary>
    /// <param name="config">Path to the widget</param>
    /// <param name="pagedArticles">Paged  list</param>
    /// <param name="index">Curent page list index</param>
    /// <returns>Generated HTML</returns>
    private static string BuildPaging(PagingConfig config, List<IEnumerable<Page>> pagedArticles, int index)
    {
        // Read the paging widget template
        var paging = new StringBuilder(FileHelper.ReadFile(config.PagingWidgetPath!));

        // Build the paging widget
        paging.Replace("@#MAX#@", pagedArticles.Count.ToString());
        paging.Replace("@#CURRENT#@", (index + 1).ToString());

        // Replace the next and previous page numbers
        if (index == pagedArticles.Count - 1)
        {
            paging.Replace("@#NEXT#@", (index + 1).ToString());
        }
        else
        {
            paging.Replace("@#NEXT#@", (index + 2).ToString());
        }

        // Replace the next and previous page numbers
        if (index == 0)
        {
            paging.Replace("@#PREVIOUS#@", (index + 1).ToString());
        }
        else
        {
            paging.Replace("@#PREVIOUS#@", index.ToString());
        }

        return paging.ToString();
    }
}
