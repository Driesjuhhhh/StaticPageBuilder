using EKO.ContentParser;

namespace EKO.StaticPageBuilder;

/// <summary>
/// A class that represents a page.
/// </summary>
internal sealed class Page
{
    /// <summary>
    /// The content of the page
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Used the template to generate the HTML.
    /// </summary>
    public string Template { get; set; } = string.Empty;

    /// <summary>
    /// The metadata of the page.
    /// </summary>
    public MarkdownMetaData MetaData { get; set; }

    /// <summary>
    /// Extra variables that can be used in the template.
    /// </summary>
    public List<PageVar> PageVars { get; set; } = new();

    /// <summary>
    /// Generated output HTML.
    /// </summary>
    public string GeneratedHTML { get; set; } = string.Empty;

    /// <summary>
    /// The path where the page will be saved.
    /// </summary>
    public string SavePath { get; set; } = string.Empty;
}
