namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public enum LauncherType
{
    VSCode = 0,
    Cursor = 1,
    Invalid = 2,
}

public static class LauncherParser
{
    public static LauncherType GetLauncher(string launcher)
    {
        launcher = launcher.Trim().ToLowerInvariant();

        return launcher switch
        {
            "code" => LauncherType.VSCode,
            "cursor" => LauncherType.Cursor,
            _ => LauncherType.Invalid,
        };
    }

    public static string GetLauncherCommand(LauncherType launcher)
    {
        return launcher switch
        {
            LauncherType.VSCode => "code",
            LauncherType.Cursor => "cursor",
            LauncherType.Invalid => "",
            _ => "",
        };
    }
}
