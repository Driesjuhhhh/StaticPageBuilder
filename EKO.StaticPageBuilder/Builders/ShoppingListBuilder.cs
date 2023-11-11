using EKO.StaticPageBuilder.Helpers;
using EKO.StaticPageBuilder.Models;

namespace EKO.StaticPageBuilder.Builders;

internal static class ShoppingListBuilder
{
    private const string IMPORTS_HEADER = "# Imports";
    private const string ACTIVE_NAVIGATION = "@#ACTIVE-";
    private const string NAVIGATION_END = "#@";

    /// <summary>
    /// Read and parse the pages. Create the correct page variables.
    /// </summary>
    /// <param name="configToReadFrom"></param>
    /// <returns></returns>
    internal static IList<Page> ReadShoppingPages(ShoppingListConfig configToReadFrom)
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
    internal static void BuildAndSavePages(ShoppingListConfig config, IList<Page> shoppingListPages)
    {
        for (int i = 0; i < shoppingListPages.Count; i++)
        {
            var template = FileHelper.ReadFile(shoppingListPages[i].Template);

            var builder = new StringBuilder(template);

            // Add the article cards to the page
            builder.Replace("@#CONTENT#@", BuildWidgets(shoppingListPages[i]));

            builder.Replace("@#TITLE#@", GetTitle(shoppingListPages[i]));

            builder.Replace("@#IMPORTS#@", GetPageImports(shoppingListPages[i]));

            AddActiveNavigation(template, shoppingListPages[i], builder);

            LogHelper.LogSuccess("Added widgets to the page");

            var fileName = shoppingListPages[i].PageVars.Find(x => x.Name == "output")?.Value ?? "index";

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

    /// <summary>
    /// Get the imorts from the page.
    /// </summary>
    /// <param name="page">Page that will be parsed</param>
    /// <returns>All required imports</returns>
    private static string GetPageImports(Page page)
    {
        var index = page.Content.IndexOf(IMPORTS_HEADER);

        if (index == -1)
        {
            return string.Empty;
        }

        // Find IMPORTS_HEADER
        var scripts = page.Content.AsSpan()[index..];

        // Skip IMPORTS_HEADER
        var pageScripts = scripts[IMPORTS_HEADER.Length..];

        // Return the scripts
        return pageScripts.ToString();
    }

    /// <summary>
    /// Get the page title
    /// </summary>
    /// <param name="page">Page that will be used.</param>
    /// <returns>Found title</returns>
    private static string GetTitle(Page page)
    {
        // Get the first line of the content
        var firstLine = page.Content.AsSpan()[..page.Content.IndexOf('\n')];

        // Skip first # and space
        return firstLine[2..].ToString();
    }

    /// <summary>
    /// Skips the Page title and the imports and returns the rest of the page.
    /// </summary>
    /// <param name="page">Page to use</param>
    /// <returns>Rest of the page</returns>
    private static string BuildWidgets(Page page)
    {
        // Skip first line
        var content = page.Content.AsSpan()[page.Content.IndexOf('\n')..];

        // Index of IMPORTS_HEADER
        var index = content.IndexOf(IMPORTS_HEADER);

        if (index != -1)
        {
            // Skip IMPORTS_HEADER
            content = content[..index];
        }

        return content.ToString();
    }

    /// <summary>
    /// Add the active class to the current page.
    /// </summary>
    /// <param name="template">Template to add the active class on</param>
    /// <param name="page">Page with the title</param>
    /// <param name="builder">Builder that will be used to replace the active tag</param>
    private static void AddActiveNavigation(string template, Page page, StringBuilder builder)
    {
        // Get the page title, remove the last character (\r) and make it uppercase
        var title = GetTitle(page)[..^1].ToUpper();

        // Get the content of the template
        var content = template.AsSpan();

        var indexes = new List<string>();

        // First index of ACTIVE_NAVIGATION
        var index = content.IndexOf(ACTIVE_NAVIGATION) + ACTIVE_NAVIGATION.Length;

        while (index != -1)
        {
            // Get the index of NAVIGATION_END
            var endIndex = content[index..].IndexOf(NAVIGATION_END);

            // If NAVIGATION_END is found
            if (endIndex != -1)
            {
                // Add the entry to the list
                indexes.Add(content[index..(index + endIndex)].ToString());

                // Remove all content that we have already checked
                content = content[(index + ACTIVE_NAVIGATION.Length + endIndex + 2)..];

                // Get the new index of ACTIVE_NAVIGATION
                index = content.IndexOf(ACTIVE_NAVIGATION) + ACTIVE_NAVIGATION.Length;
            }
            else
            {
                // If NAVIGATION_END is not found, set index to -1 to exit the loop
                index = -1;

                // Remove the entry (the rest of the template page)
                indexes = indexes[..^1];
            }

        }

        // Make the current page active
        builder.Replace(ACTIVE_NAVIGATION + title + NAVIGATION_END, "active");

        // Remove the page from the list
        indexes.Remove(title);

        // Remove the active class from the rest of the pages
        foreach (var item in indexes)
        {
            builder.Replace(ACTIVE_NAVIGATION + item + NAVIGATION_END, string.Empty);
        }
    }
}
