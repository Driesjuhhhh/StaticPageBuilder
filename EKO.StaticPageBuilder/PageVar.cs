namespace EKO.StaticPageBuilder;

/// <summary>
/// Page variable for the template. Parsed from the YAML front matter.
/// </summary>
/// <param name="Name">Name of the variable</param>
/// <param name="Value">Value of the variable</param>
internal sealed record PageVar(string Name, string Value);
