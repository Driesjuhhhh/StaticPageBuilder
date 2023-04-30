using EKO.ContentParser;
using EKO.StaticPageBuilder.Helpers;
using EKO.StaticPageBuilder.Models;

namespace EKO.StaticPageBuilder.Builders;

/// <summary>
/// Class that builds the articles pages.
/// </summary>
internal static class ArticleBuilder
{
    /// <summary>
    /// Base method that builds the article pages.
    /// </summary>
    /// <param name="config">Paths to the articles</param>
    /// <param name="files">Markdown files that will be used to create the article pages</param>
    /// <returns>All generated article pages.</returns>
    internal static List<Page> BuildPages(ArticleConfig config, List<MarkdownFile> files)
    {
        // Delete the old articles.
        DeleteArticles(config.OutputPath);

        var pages = new List<Page>();

        for (int i = 0; i < files.Count; i++)
        {
            // Add data to the page.
            var page = new Page
            {
                Content = files[i].FileContent!,
                Template = FileHelper.ReadFile(config.TemplatePath),
                MetaData = files[i].MetaData,
                SavePath = config.OutputPath,
            };

            // Add the variables to the page.
            page.PageVars.AddRange(files[i].YamlConfig.Select(x => new PageVar(x.Key, x.Value)));

            pages.Add(page);

            LogHelper.LogInfo($"Page {i + 1} of {files.Count} built. [{page.MetaData.FileName}]");
        }

        return pages;
    }

    /// <summary>
    /// Replace the variables in the template with the values from the Markdown file.
    /// </summary>
    /// <param name="pages">List of pages that will be created</param>
    internal static void SavePages(List<Page> pages)
    {
        foreach (var page in pages)
        {
            var builder = new StringBuilder(page.Template);

            // Replace the variables in the template with the values from the Markdown file.
            foreach (var pageVar in page.PageVars)
            {
                builder.Replace($"@#{pageVar.Name.ToUpper()}#@", pageVar.Value);
            }

            // Build the page with the template and the content.
            builder.Replace("@#CONTENT#@", page.Content);

            builder.Replace("@#FILENAME#@", page.MetaData.FileName);

            builder.Replace("@#DESCRIPTION#@", page.PageVars.Find(x => x.Name == "description")?.Value ?? "Blog Post");

            builder.Replace("@#COVERIMG#@", page.PageVars.Find(x => x.Name == "cover")?.Value ?? "");

            page.GeneratedHTML = builder.ToString();

            // Save the page in its own folder
            FileHelper.WriteFile(page.SavePath + page.MetaData.FileName + "/index.html", page.GeneratedHTML);

            LogHelper.LogInfo($"Page {page.MetaData.FileName}.html saved.");
        }
    }

    /// <summary>
    /// Delete the old articles.
    /// </summary>
    /// <param name="outputPath">Path to clear all files out of</param>
    private static void DeleteArticles(string outputPath)
    {
        DirectoryInfo di = new(outputPath);

        foreach (var directory in di.GetDirectories())
        {
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }

            directory.Delete();
        }
    }
}
