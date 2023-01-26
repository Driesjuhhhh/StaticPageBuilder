namespace EKO.StaticPageBuilder;

internal sealed class PageBuilderConfig
{
    public string FullPath { get; set; } = string.Empty;
    public string InputPath { get; set; } = string.Empty;
    public string OutputPath { get; set; } = string.Empty;
    public string TemplatePath { get; set; } = string.Empty;
    public string? ImagesDirectoryPath { get; set; }
    public string? WidgetPath { get; set; } // Something like a card that has to be repeated in a page
    public string? PagingWidgetPath { get; set; } // Element that shows the current page and allows to navigate to other pages
    public bool HasContent { get; set; }
    public List<PageVar> PageVars { get; set; } = new();
}
