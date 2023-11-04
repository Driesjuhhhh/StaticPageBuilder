namespace EKO.StaticPageBuilder.Helpers;

/// <summary>
/// Watches the site directory for changes and rebuilds the site when changes are detected.
/// </summary>
internal static class SiteDirectoryWatcher
{
    /// <summary>
    /// The function that will be called when changes are detected.
    /// </summary>
    private static Action<string>? _builderFunction;

    /// <summary>
    /// The path to watch.
    /// </summary>
    private static string? _path;

    /// <summary>
    /// The FileSystemWatcher instance.
    /// </summary>
    private static FileSystemWatcher? _watcher;

    /// <summary>
    /// Starts watching the given directory and subdirectories for changes.
    /// </summary>
    /// <param name="path">Path of the directory to watch for changes</param>
    /// <param name="func">Function to run when a change is detected</param>
    internal static void WatchDirectory(string path, Action<string> func)
    {
        _watcher = new FileSystemWatcher
        {
            Path = path,
            NotifyFilter = NotifyFilters.Size,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _builderFunction = func;

        _path = path;

        _watcher.Changed += Files_Changed;
    }

    /// <summary>
    /// Called when changes are detected in the directory.
    /// </summary>
    private static void Files_Changed(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            return;
        }

        Console.WriteLine($"Changes found: {e.FullPath}");

        // Disable the watcher so we don't get stuck in a loop where each rebuild triggers another rebuild.
        _watcher!.EnableRaisingEvents = false;

        Console.WriteLine("**************************************");
        Console.WriteLine("**************************************");
        Console.WriteLine("**                                  **");
        Console.WriteLine("**         Rebuilding Site...       **");
        Console.WriteLine("**                                  **");
        Console.WriteLine("**************************************");
        Console.WriteLine("**************************************");

        // Wait for the file to be written.
        Thread.Sleep(250);

        if (_builderFunction is null || _path is null)
        {
            Console.WriteLine("Passed function or Path was null!");
            return;
        }

        // Rebuild the site.
        _builderFunction(_path);

        // Re-enable the watcher.
        _watcher.EnableRaisingEvents = true;
    }
}
