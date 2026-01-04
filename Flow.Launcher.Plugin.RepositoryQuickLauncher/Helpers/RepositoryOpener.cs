using System;
using System.Diagnostics;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

public static class RepositoryOpener
{
    public static void OpenFolder(
        Repository repository,
        PluginInitContext context,
        Settings settings
    )
    {
        if (repository.IsWsl)
        {
            OpenWslFolder(repository, context, settings);
        }
        else
        {
            OpenWindowsFolder(repository, context, settings);
        }
    }

    private static void OpenWslFolder(
        Repository repository,
        PluginInitContext context,
        Settings settings
    )
    {
        ProcessStartInfo processStartInfo = new()
        {
            FileName = "wsl.exe",
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        processStartInfo.ArgumentList.Add("--distribution");
        processStartInfo.ArgumentList.Add(settings.WslDistributionName);

        processStartInfo.ArgumentList.Add(settings.WslLaunchCommand);

        processStartInfo.ArgumentList.Add($"'{repository.WslPath}'");

        try
        {
            _ = Process.Start(processStartInfo);
        }
        catch (Exception ex)
        {
            context.API.ShowMsg($"Error opening folder {repository.Name}", ex.Message);
        }
    }

    private static void OpenWindowsFolder(
        Repository repository,
        PluginInitContext context,
        Settings settings
    )
    {
        ProcessStartInfo processStartInfo = new()
        {
            FileName = settings.WindowsLaunchCommand,
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        processStartInfo.ArgumentList.Add(repository.Path);

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
