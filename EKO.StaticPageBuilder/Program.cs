using EKO.StaticPageBuilder;
using EKO_ContentParser;

var path = "D:\\Code\\Sites\\ekozf.github.io";

var config = PathChecker.CheckAndConvertArgs(new string[] { path });

if (config is null) return;

var pathsAreValid = PathChecker.IsPathValid(config.InputPath) &&
                    PathChecker.IsPathValid(config.OutputPath) &&
                    PathChecker.IsPathValid(config.TemplatePath) &&
                    PathChecker.IsPathValid(config.ImagesDirectoryPath);

if (pathsAreValid)
{
    var generator = MarkdownParser.GetParser();

    var results = generator
                    .UseFileOrDirectory(config.InputPath)
                    .ReadContents()
                    .ParseMarkdown()
                    .ParseYaml()
                    .SaveFilesTo(config.OutputPath)
                    .GetObjects();

    if (results != null)
    {
        Console.WriteLine("Parsed " + results.Count + " files");

        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];

            var pairs = result.YamlConfig.ToList();

            config.PageVars.Add(new PageVar(pairs[i].Key, pairs[i].Value));
        }

        //add filename, createdate, editdate
        // remember, input for this program should be an array of PageBuilderConfig
    }
}
