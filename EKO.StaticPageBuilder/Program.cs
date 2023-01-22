using EKO.StaticPageBuilder;
using EKO_ContentParser;
using System.Text;

var path = "D:\\Code\\Sites\\ekozf.github.io";

var configs = PathChecker.CheckAndConvertArgs(new string[] { path });

if (configs is null) return;

var pages = new List<Page>();

foreach (var config in configs.Where(c => c.HasContent))
{
    var pathsAreValid = PathChecker.IsPathValid(config.InputPath) &&
                        PathChecker.IsPathValid(config.OutputPath) &&
                        PathChecker.IsPathValid(config.TemplatePath);

    if (pathsAreValid)
    {
        var generator = MarkdownParser.GetParser();

        var results = generator
                        .UseFileOrDirectory(config.InputPath)
                        .ReadContents()
                        .ParseMarkdown()
                        .ParseYaml()
                        .SaveFilesTo(config.OutputPath)
                        .GetObjects();

        if (results != null)
        {
            Console.WriteLine("Parsed " + results.Count + " files");

            for (int i = 0; i < results.Count; i++)
            {
                var page = new Page
                {
                    Content = results[i].FileContent!,
                    Template = File.ReadAllText(config.TemplatePath),
                    MetaData = results[i].MetaData,
                    SavePath = config.OutputPath,
                };

                page.PageVars.AddRange(results[i].YamlConfig.Select(x => new PageVar(x.Key, x.Value)));

                pages.Add(page);
            }

            Console.WriteLine("Done loading in pages & variables.");
        }
    }
}

foreach (var page in pages)
{
    var builder = new StringBuilder(page.Template);

    foreach (var pageVar in page.PageVars)
    {
        builder.Replace($"@#{pageVar.Name.ToUpper()}#@", pageVar.Value);
    }

    builder.Replace("@#CONTENT#@", page.Content);

    page.GeneratedHTML = builder.ToString();

    File.WriteAllText(page.SavePath + page.MetaData.FileName + ".html", page.GeneratedHTML);
}

foreach (var config in configs.Where(c => !c.HasContent))
{
    var paged = Helpers.PageList(pages, 5);

    for (int i = 0; i < paged.Count; i++)
    {
        var builder = new StringBuilder(File.ReadAllText(config.TemplatePath));

        var widgetList = new StringBuilder();

        foreach (var page in paged[i])
        {
            var widget = File.ReadAllText(config.WidgetPath!);

            widget = widget
                        .Replace("@#IMAGESDIRECTORYPATH#@", config.ImagesDirectoryPath)
                        .Replace("@#COVER#@", page.PageVars.Find(x => x.Name == "cover")?.Value ?? "code.jpg")
                        .Replace("@#TITLE#@", page.PageVars.Find(x => x.Name == "title")?.Value ?? "No title")
                        .Replace("@#DESCRIPTION#@", page.PageVars.Find(x => x.Name == "description")?.Value ?? "No description")
                        .Replace("@#FILENAME#@", page.MetaData.FileName);

            widgetList.Append(widget);
        }

        builder.Replace("@#WIDGETS#@", widgetList.ToString());

        var t = builder.ToString();
        var b = widgetList.ToString();

        File.WriteAllText(config.OutputPath + $"{i + 1}.html", builder.ToString());
    }
}

Console.WriteLine("Done building pages.");
Console.ReadLine();