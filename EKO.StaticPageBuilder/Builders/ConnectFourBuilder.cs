using EKO.StaticPageBuilder.Helpers;
using EKO.StaticPageBuilder.Models;

namespace EKO.StaticPageBuilder.Builders;

internal static class ConnectFourBuilder
{
    internal static IList<Page> ReadGamePages(ConnectFourConfig configToReadFrom)
    {
        // Get all files in the directory
        var files = Directory.GetFiles(configToReadFrom.InputPath);

        var pages = new List<Page>();

        foreach (var file in files)
        {
            var page = new Page
            {
                Content = FileHelper.ReadFile(file),
                GeneratedHTML = string.Empty,
                SavePath = configToReadFrom.OutputPath,
                Template = configToReadFrom.TemplatePath,
            };
        
            page.PageVars.Add(new PageVar("output", Path.GetFileNameWithoutExtension(file)));

            pages.Add(page);
        }

        return pages;
    }


    /// <summary>
    /// Builds the pages for the paging.
    /// </summary>
    /// <param name="config">Paths to the pages</param>
    /// <param name="pagedArticles">Paged list of pages</param>
    internal static void BuildAndSavePages(ConnectFourConfig config, IList<Page> connectFourPages)
    {
        for (int i = 0; i < connectFourPages.Count; i++)
        {
            var builder = new StringBuilder(FileHelper.ReadFile(config.TemplatePath));

            // Add the article cards to the page
            builder.Replace("@#CONTENT#@", BuildWidgets(connectFourPages[i]));

            builder.Replace("@#TITLE#@", AddTitle(connectFourPages[i]));

            builder.Replace("@#SCRIPTS#@", GetPageScripts(connectFourPages[i]));

            LogHelper.LogSuccess("Added widgets to the page");

            var fileName = connectFourPages[i].PageVars.Find(x => x.Name == "output")?.Value ?? "index";

            // Save the page.
            if (fileName == "index")
            {
                LogHelper.LogInfo("Building index page");
                FileHelper.WriteFile(config.OutputPath + $"index.html", builder.ToString());
            }
            else
            {
                LogHelper.LogInfo($"Building page {fileName}");
                FileHelper.WriteFile(config.OutputPath + $"{fileName}/index.html", builder.ToString());
            }
        }
    }

    private static string GetPageScripts(Page page)
    {
        var index = page.Content.IndexOf("# Scripts");

        if (index == -1)
        {
            return string.Empty;
        }

        // Find '# Scripts'
        var scripts = page.Content.AsSpan()[index..];

        // Skip '# Scripts'
        var pageScripts = scripts[10..];

        // Return the scripts
        return pageScripts.ToString();
    }

    private static string AddTitle(Page page)
    {
        // Get the first line of the content
        var firstLine = page.Content.AsSpan()[..page.Content.IndexOf('\n')];

        // Skip first # and space
        return firstLine[2..].ToString();
    }

    private static string BuildWidgets(Page page)
    {
        // Skip first line
        var content = page.Content.AsSpan()[page.Content.IndexOf('\n')..];

        // Index of '# Scripts'
        var index = content.IndexOf("# Scripts");

        if (index != -1)
        {
            // Skip '# Scripts'
            content = content[..index];
        }

        return content.ToString();
    }
}
