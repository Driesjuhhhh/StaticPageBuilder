using EKO.ContentParser;
using EKO.StaticPageBuilder.Helpers;
using EKO.StaticPageBuilder.Models;

namespace EKO.StaticPageBuilder.Builders;

internal static class ArticleBuilder
{
    internal static List<Page> BuildPages(ArticleConfig config, List<MarkdownFile> files)
    {
        DeleteArticles(config.OutputPath);

        var pages = new List<Page>();

        for (int i = 0; i < files.Count; i++)
        {
            var page = new Page
            {
                Content = files[i].FileContent!,
                Template = FileHelper.ReadFile(config.TemplatePath),
                MetaData = files[i].MetaData,
                SavePath = config.OutputPath,
            };

            page.PageVars.AddRange(files[i].YamlConfig.Select(x => new PageVar(x.Key, x.Value)));

            pages.Add(page);

            LogHelper.LogInfo($"Page {i + 1} of {files.Count} built. [{page.MetaData.FileName}]");
        }

        return pages;
    }

    internal static void SavePages(List<Page> pages)
    {
        foreach (var page in pages)
        {
            var builder = new StringBuilder(page.Template);

            foreach (var pageVar in page.PageVars)
            {
                builder.Replace($"@#{pageVar.Name.ToUpper()}#@", pageVar.Value);
            }

            builder.Replace("@#CONTENT#@", page.Content);

            builder.Replace("@#FILENAME#@", page.MetaData.FileName);

            builder.Replace("@#DESCRIPTION#@", page.PageVars.Find(x => x.Name == "description")?.Value ?? "Blog Post");

            builder.Replace("@#COVERIMG#@", page.PageVars.Find(x => x.Name == "cover")?.Value ?? "");

            page.GeneratedHTML = builder.ToString();

            FileHelper.WriteFile(page.SavePath + page.MetaData.FileName + "/index.html", page.GeneratedHTML);

            LogHelper.LogInfo($"Page {page.MetaData.FileName}.html saved.");
        }
    }

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
