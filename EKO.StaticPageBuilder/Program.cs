using EKO.StaticPageBuilder.Builders;
using EKO.StaticPageBuilder.Helpers;
using EKO_ContentParser;

var path = @"D:\Code\Sites\ekozf.github.io";

var configFile = ConfigHelper.LocateConfigFile(path);

if (string.IsNullOrWhiteSpace(configFile))
{
    LogHelper.LogError("Couldn't locate the config file.");
    return;
}

LogHelper.LogSuccess("Config file found.");
var jsonContent = FileHelper.ReadFile(configFile);

if (string.IsNullOrWhiteSpace(jsonContent))
{
    LogHelper.LogError("Couldn't read the config file.");
    return;
}

LogHelper.LogSuccess("Successfully read the config file.");
var config = ConfigHelper.ParseConfig(jsonContent);

if (config is null)
{
    LogHelper.LogError("Couldn't parse the config file.");
    return;
}

LogHelper.LogSuccess("Parsed the config file.");
ConfigHelper.AdjustConfigPaths(config, configFile);

var generator = MarkdownParser.GetParser();

var results = generator
                .UseFileOrDirectory(config.Article.InputPath)
                .ReadContents()
                .ParseMarkdown()
                .ParseYaml()
                .SaveFilesTo(config.Article.OutputPath)
                .GetObjects();

if (results == null)
{
    LogHelper.LogError("Failed to convert Markdown files to HTML.");
    return;
}

LogHelper.LogSuccess("Read & converted Markdown files to HTML.");
LogHelper.LogInfo($"Parsed {results.Count} files.");

var articles = ArticleBuilder.BuildPages(config.Article, results).OrderByDescending(x => x.MetaData.CreateDate).ToList();
ArticleBuilder.SavePages(articles);
LogHelper.LogInfo("Loaded, created, and saved page objects.");

var pagedArticles = ListHelper.PageList(articles, 5);
PagingBuilder.BuildAndSavePages(config.Paging, pagedArticles);
LogHelper.LogInfo("Built and saved paged pages.");

HomeBuilder.BuildAndSavePages(config.Home, articles.OrderByDescending(x => x.MetaData.CreateDate).Take(5));
LogHelper.LogInfo("Built home page.");

LogHelper.LogInfo("Finished building site.");
Console.ReadLine();