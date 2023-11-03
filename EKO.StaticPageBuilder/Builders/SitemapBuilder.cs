using EKO.StaticPageBuilder.Helpers;

namespace EKO.StaticPageBuilder.Builders;

/// <summary>
/// Build the Sitemap
/// </summary>
internal static class SitemapBuilder
{
    /// <summary>
    /// Files to ignore when generating the sitemap.
    /// </summary>
    private static readonly string[] IGNORED_FILES = ["template", "node", "widgets", "/source/"];

    /// <summary>
    /// Read all HTML files and generate a sitemap.
    /// </summary>
    /// <param name="path"></param>
    internal static void GenerateSitemap(string path)
    {
        var builder = new StringBuilder();

        // Get all HTML files.
        foreach (var page in Directory.GetFiles(path, "*.html", SearchOption.AllDirectories))
        {
            // Get the link to the page.
            var link = page.Replace(path, "").Replace('\\', '/').Replace("index.html", "");

            if (!ContainsOneOf(IGNORED_FILES, link))
            {
                builder.Append("https://emirkaan.be").AppendLine(link);

            }
        }

        // Write the sitemap to a file.
        FileHelper.WriteFile(path + @"\sitemap.txt", builder.ToString());
    }

    private static bool ContainsOneOf(string[] array, string value)
    {
        foreach (var item in array)
        {
            if (value.Contains(item, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
