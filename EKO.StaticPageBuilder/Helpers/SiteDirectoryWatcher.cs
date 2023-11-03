namespace EKO.StaticPageBuilder.Helpers;

internal static class SiteDirectoryWatcher
{
    private static Action<string>? _builderFunction;
    private static string? _path;
    private static FileSystemWatcher? _watcher;

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

    private static void Files_Changed(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            return;
        }

        Console.WriteLine($"Changes found: {e.FullPath}");

        _watcher!.EnableRaisingEvents = false;

        Console.WriteLine("**************************************");
        Console.WriteLine("**************************************");
        Console.WriteLine("**                                  **");
        Console.WriteLine("**         Rebuilding Site...       **");
        Console.WriteLine("**                                  **");
        Console.WriteLine("**************************************");
        Console.WriteLine("**************************************");

        Thread.Sleep(250);

        if (_builderFunction is null || _path is null)
        {
            Console.WriteLine("Passed function or Path was null!");
            return;
        }

        _builderFunction(_path);

        _watcher.EnableRaisingEvents = true;
    }
}
