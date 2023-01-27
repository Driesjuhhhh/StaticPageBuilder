namespace EKO.StaticPageBuilder.Models;

internal sealed class HomeConfig : BaseConfig
{
    public string ImagesDirectoryPath { get; set; } = string.Empty;
    public string WidgetPath { get; set; } = string.Empty; // Something like a card that has to be repeated in a page
}
