namespace EKO.StaticPageBuilder.Models;

/// <summary>
/// Base class for the config files.
/// </summary>
internal abstract class BaseConfig
{
    /// <summary>
    /// Input path for the files
    /// </summary>
    public string InputPath { get; set; } = string.Empty;

    /// <summary>
    /// Output path of the generated files
    /// </summary>
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>
    /// Path to the template that will be used to generate the pages
    /// </summary>
    public string TemplatePath { get; set; } = string.Empty;
}
