using System.Collections.Generic;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

public static class Messages
{
    public static List<Result> GetLoadingMessage()
    {
        Result loadingResult = new()
        {
            Title = "Discovering repositories...",
            SubTitle = "Please wait...",
            IcoPath = Constants.IconPath,
        };

        return new List<Result>() { loadingResult };
    }

    public static List<Result> GetMissingDirectoriesMessage(PluginInitContext? context)
    {
        Result missingDirectoriesResult = new()
        {
            Title = "Missing Configuration",
            SubTitle = "Please provide a Windows or WSL directory in the plugin settings",
            Score = 100,
            IcoPath = Constants.IconPath,
            Action = (e) =>
            {
                context?.API.OpenSettingDialog();
                return true;
            },
        };

        Result reloadPluginResult = GetReloadPluginResult(context);

        return new List<Result>() { missingDirectoriesResult, reloadPluginResult };
    }

    public static List<Result> GetMissingLaunchCommandsMessage(PluginInitContext? context)
    {
        Result missingLaunchCommandsResult = new()
        {
            Title = "Missing Launch Commands",
            SubTitle = "Please provide a Windows or WSL launch command in the plugin settings",
            Score = 100,
            IcoPath = Constants.IconPath,
            Action = (e) =>
            {
                context?.API.OpenSettingDialog();
                return true;
            },
        };

        Result reloadPluginResult = GetReloadPluginResult(context);

        return new List<Result>() { missingLaunchCommandsResult, reloadPluginResult };
    }

    public static List<Result> GetNoResultsMessage(PluginInitContext? context)
    {
        Result noResultsResult = new()
        {
            Title = "No results",
            SubTitle = "No results found",
            Score = 100,
            IcoPath = Constants.IconPath,
        };

        Result reloadPluginResult = GetReloadPluginResult(context);

        return new List<Result>() { noResultsResult, reloadPluginResult };
    }

    private static Result GetReloadPluginResult(PluginInitContext? context)
    {
        Result reloadPluginResult = new()
        {
            Title = "Reload plugin",
            SubTitle = "Reload plugin",
            IcoPath = Constants.IconPath,
            Score = 1,
            Action = (e) =>
            {
                _ = context?.API.ReloadAllPluginData();
                return true;
            },
        };

        return reloadPluginResult;
    }
}
