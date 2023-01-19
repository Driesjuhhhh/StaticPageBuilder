namespace EKO.StaticPageBuilder;

internal sealed class PageBuilderConfig
{
    public string FullPath { get; set; } = string.Empty;
    public string InputPath { get; set; } = string.Empty;
    public string OutputPath { get; set; } = string.Empty;
    public string TemplatePath { get; set; } = string.Empty;
    public string ImagesDirectoryPath { get; set; } = string.Empty;
    public List<PageVar> PageVars { get; set; } = new();
}
