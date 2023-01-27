namespace EKO.StaticPageBuilder.Models;

internal sealed class PagingConfig : BaseConfig
{
    public string ImagesDirectoryPath { get; set; } = string.Empty;
    public string WidgetPath { get; set; } = string.Empty;// Something like a card that has to be repeated in a page
    public string PagingWidgetPath { get; set; } = string.Empty; // Element that shows the current page and allows to navigate to other pages
}
