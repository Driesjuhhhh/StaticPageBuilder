using EKO.StaticPageBuilder.Helpers;
using EKO.StaticPageBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace EKO.StaticPageBuilder.Builders;

internal static class PagingBuilder
{
    internal static void BuildAndSavePages(PagingConfig config, List<IEnumerable<Page>> pagedArticles)
    {
        for (int i = 0; i < pagedArticles.Count; i++)
        {
            var builder = new StringBuilder(FileHelper.ReadFile(config.TemplatePath));

            builder.Replace("@#WIDGETS#@", BuildWidgets(config, pagedArticles[i]));
            LogHelper.LogSuccess("Added widgets to the page");

            builder.Replace("@#PAGING#@", BuildPaging(config, pagedArticles, i));
            LogHelper.LogSuccess("Added paging widget to the page.");

            FileHelper.WriteFile(config.OutputPath + $"{i + 1}.html", builder.ToString());
        }
    }

    private static string BuildWidgets(PagingConfig config, IEnumerable<Page> articles)
    {
        var widgetList = new StringBuilder();

        foreach (var page in articles)
        {
            var widget = FileHelper.ReadFile(config.WidgetPath);

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

    private static string BuildPaging(PagingConfig config, List<IEnumerable<Page>> pagedArticles, int index)
    {
        var paging = new StringBuilder(FileHelper.ReadFile(config.PagingWidgetPath!));

        paging.Replace("@#MAX#@", pagedArticles.Count.ToString());
        paging.Replace("@#CURRENT#@", (index + 1).ToString());

        if (index == pagedArticles.Count - 1)
        {
            paging.Replace("@#NEXT#@", (index + 1).ToString());
        }
        else
        {
            paging.Replace("@#NEXT#@", (index + 2).ToString());
        }

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
