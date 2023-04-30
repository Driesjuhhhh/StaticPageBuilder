namespace EKO.StaticPageBuilder.Helpers;

/// <summary>
/// Class for logging messages to the console.
/// </summary>
internal static class LogHelper
{
    /// <summary>
    /// Log an info message to the console.
    /// </summary>
    /// <param name="msg"></param>
    internal static void LogInfo(string msg)
    {
        Console.WriteLine("[INFO] " + msg);
    }

    /// <summary>
    /// Log a warning message to the console.
    /// </summary>
    /// <param name="msg"></param>
    internal static void LogWarning(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[WARN] " + msg);
        Console.ResetColor();
    }

    /// <summary>
    /// Log an error message to the console.
    /// </summary>
    /// <param name="msg"></param>
    internal static void LogError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERROR] " + msg);
        Console.ResetColor();
    }

    /// <summary>
    /// Log a success message to the console.
    /// </summary>
    /// <param name="msg"></param>
    internal static void LogSuccess(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[SUCCESS] " + msg);
        Console.ResetColor();
    }
}
