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
    /// <param name="path">Path to the root directory that will be used to generate a sitemap</param>
    internal static void GenerateSitemap(string path)
    {
        var builder = new StringBuilder();

        // Get all HTML files.
        foreach (var page in Directory.GetFiles(path, "*.html", SearchOption.AllDirectories))
        {
            // Get the link to the page.
            var link = page.Replace(path, "").Replace('\\', '/').Replace("index.html", "");

            // Check if the link is not in the ignored files.
            if (!ContainsOneOf(IGNORED_FILES, link))
            {
                builder.Append("https://emirkaan.be").AppendLine(link);

            }
        }

        // Write the sitemap to a file.
        FileHelper.WriteFile(path + @"\sitemap.txt", builder.ToString());
    }

    /// <summary>
    /// Check if the string is equal to one of the strings in the array.
    /// </summary>
    /// <param name="array">Array to check</param>
    /// <param name="value">Value to find in the array</param>
    /// <returns>true if the value was found in the array, otherwise false</returns>
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
