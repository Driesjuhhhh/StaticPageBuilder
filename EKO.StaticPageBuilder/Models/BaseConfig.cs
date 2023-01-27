namespace EKO.StaticPageBuilder.Models;

internal abstract class BaseConfig
{
    public string InputPath { get; set; } = string.Empty;
    public string OutputPath { get; set; } = string.Empty;
    public string TemplatePath { get; set; } = string.Empty;
}
