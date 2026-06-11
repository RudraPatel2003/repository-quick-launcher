using System;
using System.Diagnostics;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

public static class RepositoryOpener
{
    public static void OpenFolder(
        Repository repository,
        PluginInitContext context,
        CommandSetting command
    )
    {
        if (FileExplorerHelper.ShouldOpenInFileExplorer(repository, command))
        {
            FileExplorerHelper.OpenFolderInFileExplorer(repository, context);

            return;
        }

        if (repository.IsWsl)
        {
            OpenWslFolder(repository, context, command);
        }
        else
        {
            OpenWindowsFolder(repository, context, command);
        }
    }

    private static void OpenWslFolder(
        Repository repository,
        PluginInitContext context,
        CommandSetting command
    )
    {
        ProcessStartInfo processStartInfo = new()
        {
            FileName = "wsl.exe",
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        processStartInfo.ArgumentList.Add("--distribution");
        processStartInfo.ArgumentList.Add(command.WslDistributionName);

        processStartInfo.ArgumentList.Add(command.WslLaunchCommand);

        processStartInfo.ArgumentList.Add(repository.WslPath);

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
        CommandSetting command
    )
    {
        ProcessStartInfo processStartInfo = new()
        {
            FileName = command.WindowsLaunchCommand,
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
