namespace EKO.StaticPageBuilder.Helpers;

internal static class LogHelper
{
    internal static void LogInfo(string msg)
    {
        Console.WriteLine("[INFO] " + msg);
    }

    internal static void LogWarning(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[WARN] " + msg);
        Console.ResetColor();
    }

    internal static void LogError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERROR] " + msg);
        Console.ResetColor();
    }

    internal static void LogSuccess(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[SUCCESS] " + msg);
        Console.ResetColor();
    }
}
