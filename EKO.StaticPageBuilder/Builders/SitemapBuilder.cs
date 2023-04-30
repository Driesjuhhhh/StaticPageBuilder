using EKO.StaticPageBuilder.Helpers;

namespace EKO.StaticPageBuilder.Builders;

/// <summary>
/// Build the Sitemap
/// </summary>
internal static class SitemapBuilder
{
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

            // Skip pages that are not articles.
            if (!link.Contains("template", StringComparison.OrdinalIgnoreCase) && !link.Contains("node", StringComparison.Ordinal))
            {
                builder.Append("https://blog.emirkaan.be").AppendLine(link);
            }
        }

        // Write the sitemap to a file.
        FileHelper.WriteFile(path + @"\sitemap.txt", builder.ToString());
    }
}
