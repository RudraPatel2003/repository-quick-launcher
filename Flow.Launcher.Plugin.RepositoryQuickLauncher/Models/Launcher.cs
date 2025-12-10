namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public enum LauncherEnumeration
{
    VSCode = 0,
    Cursor = 1,
    Invalid = 2,
}

public static class LauncherExtensions
{
    public static LauncherEnumeration GetLauncher(string launcher)
    {
        launcher = launcher.Trim().ToLowerInvariant();

        if (launcher == "code")
        {
            return LauncherEnumeration.VSCode;
        }

        if (launcher == "cursor")
        {
            return LauncherEnumeration.Cursor;
        }

        return LauncherEnumeration.Invalid;
    }

    public static string GetLauncherCommand(LauncherEnumeration launcher)
    {
        if (launcher == LauncherEnumeration.VSCode)
        {
            return "code";
        }

        if (launcher == LauncherEnumeration.Cursor)
        {
            return "cursor";
        }

        return "";
    }
}
