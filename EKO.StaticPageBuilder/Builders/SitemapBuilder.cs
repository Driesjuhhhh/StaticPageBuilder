using EKO.StaticPageBuilder.Helpers;
using System.Text;

namespace EKO.StaticPageBuilder.Builders;

internal static class SitemapBuilder
{
    internal static void GenerateSitemap(string path)
    {
        var builder = new StringBuilder();

        foreach (var page in Directory.GetFiles(path, "*.html", SearchOption.AllDirectories))
        {
            var link = page.Replace(path, "").Replace('\\', '/').Replace("index.html", "");

            if (!link.Contains("template", StringComparison.OrdinalIgnoreCase) && !link.Contains("node", StringComparison.Ordinal))
            {
                builder.Append("https://blog.emirkaan.be").AppendLine(link);
            }
        }

        FileHelper.WriteFile(path + @"\sitemap.txt", builder.ToString());
    }
}
