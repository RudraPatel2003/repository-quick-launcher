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

    public static List<Result> GetMissingConfigurationMessage(PluginInitContext? context)
    {
        Result missingConfigurationResult = new()
        {
            Title = "Missing Configuration",
            SubTitle = "Please provide a Windows or WSL directory in the plugin settings",
            IcoPath = Constants.IconPath,
            Action = (e) =>
            {
                context?.API.OpenSettingDialog();
                return true;
            },
        };

        Result reloadPluginResult = GetReloadPluginResult(context);

        return new List<Result>() { missingConfigurationResult, reloadPluginResult };
    }

    public static List<Result> GetEnterCommandMessage()
    {
        Result enterCommandResult = new()
        {
            Title = "Enter command",
            SubTitle =
                """Please type "code" or "cursor" and then you search query to begin searching for repositories""",
            IcoPath = Constants.IconPath,
        };

        return new List<Result>() { enterCommandResult };
    }

    public static List<Result> GetInvalidCommandMessage()
    {
        Result invalidCommandResult = new()
        {
            Title = "Invalid command",
            SubTitle =
                """Please type "code" or "cursor" and then you search query to begin searching for repositories""",
            IcoPath = Constants.IconPath,
        };

        return new List<Result>() { invalidCommandResult };
    }

    public static List<Result> GetNoResultsMessage(PluginInitContext? context)
    {
        Result noResultsResult = new()
        {
            Title = "No results",
            SubTitle = "No results found",
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
            Action = (e) =>
            {
                _ = context?.API.ReloadAllPluginData();
                return true;
            },
        };

        return reloadPluginResult;
    }
}
