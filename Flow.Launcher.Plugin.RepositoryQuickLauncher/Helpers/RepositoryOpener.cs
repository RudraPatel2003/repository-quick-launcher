using System;
using System.Diagnostics;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

public static class RepositoryOpener
{
    public static void OpenFolder(
        LauncherType launcher,
        Repository repository,
        PluginInitContext context,
        Settings settings
    )
    {
        string launchCommand = LauncherParser.GetLauncherCommand(launcher);

        if (string.IsNullOrWhiteSpace(launchCommand))
        {
            return;
        }

        string command = $"{launchCommand} {repository.Path}";

        if (repository.IsWsl)
        {
            string wslDistribution = settings.WslDistributionName;

            command =
                $"wsl --distribution {wslDistribution} --cd {repository.Directory} {launchCommand} {repository.Name}";
        }

        ProcessStartInfo processStartInfo = new()
        {
            FileName = "cmd.exe",
            Arguments = $"/c \"{command}\"",
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        try
        {
            _ = Process.Start(processStartInfo);
        }
        catch (Exception ex)
        {
            context.API.ShowMsg($"Error opening folder {repository.Name}", ex.Message);
        }
    }
}
