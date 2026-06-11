using System;
using System.Diagnostics;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

/// <summary>
/// File Explorer is a special case as
/// 1. It does not use ProcessWindowStyle.Hidden
/// 2. Even for WSL repositories, you want to run the explorer command in Windows
///
/// This helper class handles this case
/// </summary>
public static class FileExplorerHelper
{
    public static bool ShouldOpenInFileExplorer(Repository repository, CommandSetting command)
    {
        if (repository.IsWsl && IsFileExplorerCommand(command.WslLaunchCommand))
        {
            return true;
        }

        return IsFileExplorerCommand(command.WindowsLaunchCommand);
    }

    public static void OpenFolderInFileExplorer(Repository repository, PluginInitContext context)
    {
        try
        {
            _ = Process.Start(
                new ProcessStartInfo
                {
                    FileName = Constants.ExplorerExecutable,
                    Arguments = repository.Path,
                    UseShellExecute = true,
                }
            );
        }
        catch (Exception ex)
        {
            context.API.ShowMsg($"Error opening folder {repository.Name}", ex.Message);
        }
    }

    private static bool IsFileExplorerCommand(string launchCommand)
    {
        string formattedLaunchCommand = launchCommand.Trim();

        return formattedLaunchCommand.Equals(Constants.ExplorerCommand, StringComparison.Ordinal)
            || formattedLaunchCommand.Equals(
                Constants.ExplorerExecutable,
                StringComparison.Ordinal
            );
    }
}
