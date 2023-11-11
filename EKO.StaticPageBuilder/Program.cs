using EKO.ContentParser;
using EKO.StaticPageBuilder.Builders;
using EKO.StaticPageBuilder.Helpers;

static void BuildSite(string path)
{

    // Get the config file.
    var configFile = ConfigHelper.LocateConfigFile(path);

    if (string.IsNullOrWhiteSpace(configFile))
    {
        LogHelper.LogError("Couldn't locate the config file.");
        return;
    }

    // Read the config file.
    LogHelper.LogSuccess("Config file found.");
    var jsonContent = FileHelper.ReadFile(configFile);

    if (string.IsNullOrWhiteSpace(jsonContent))
    {
        LogHelper.LogError("Couldn't read the config file.");
        return;
    }

    // Parse the config file.
    LogHelper.LogSuccess("Successfully read the config file.");
    var config = ConfigHelper.ParseConfig(jsonContent);

    if (config is null)
    {
        LogHelper.LogError("Couldn't parse the config file.");
        return;
    }

    // Adjust the config paths to the correct paths.
    LogHelper.LogSuccess("Parsed the config file.");
    ConfigHelper.AdjustConfigPaths(config, configFile);

    // Get the Markdown files and convert them to HTML.
    var generator = MarkdownParser.GetParser();

    var results = generator
                    .UseFileOrDirectory(config.Article.InputPath)
                    .ReadContents()
                    .ParseMarkdown()
                    .ParseYaml()
                    .GetObjects();

    // Check if the Markdown files were converted to HTML.
    if (results == null)
    {
        LogHelper.LogError("Failed to convert Markdown files to HTML.");
        return;
    }

    LogHelper.LogSuccess("Read & converted Markdown files to HTML.");
    LogHelper.LogInfo($"Parsed {results.Count} files.");

    // Build the article pages
    var articles = ArticleBuilder.BuildPages(config.Article, results).OrderByDescending(x => x.MetaData.CreateDate).ToList();
    ArticleBuilder.SavePages(articles);
    LogHelper.LogInfo("Loaded, created, and saved page objects.");

    // Build the multipage blog
    var pagedArticles = ListHelper.PageList(articles, 5);
    PagingBuilder.BuildAndSavePages(config.Paging, pagedArticles);
    LogHelper.LogInfo("Built and saved paged pages.");

    // Build the start page
    HomeBuilder.BuildAndSavePages(config.Home, articles.OrderByDescending(x => x.MetaData.CreateDate).Take(5));
    LogHelper.LogInfo("Built home page.");

    LogHelper.LogInfo("Building connect four game...");

    var connectFourPages = ConnectFourBuilder.ReadGamePages(config.ConnectFour);

    LogHelper.LogInfo("Read Connect Four Pages. Building pages.");

    ConnectFourBuilder.BuildAndSavePages(config.ConnectFour, connectFourPages);

    LogHelper.LogInfo("Finished building connect four game.");

    LogHelper.LogInfo("Building shopping list...");
    ShoppingListBuilder.ReadShoppingPages(config.ShoppingList);

    LogHelper.LogInfo("Read Shopping List Pages. Building pages.");

    ShoppingListBuilder.BuildAndSavePages(config.ShoppingList, ShoppingListBuilder.ReadShoppingPages(config.ShoppingList));

    LogHelper.LogInfo("Finished building shopping list.");

    LogHelper.LogInfo("Finished building site.");

    // Generate the sitemap
    LogHelper.LogInfo("Generating sitemap.");
    SitemapBuilder.GenerateSitemap(path);
    LogHelper.LogSuccess("Generated sitemap.");

    LogHelper.LogSuccess("Finished all tasks, quitting...");

    LogHelper.LogInfo("Press ctrl + c to quit...");
}

// Path to the folder where the config.json file is located.
var path = @"D:\Code\Sites\ekozf.github.io";

BuildSite(path);

SiteDirectoryWatcher.WatchDirectory(path, BuildSite);

Console.ReadLine();