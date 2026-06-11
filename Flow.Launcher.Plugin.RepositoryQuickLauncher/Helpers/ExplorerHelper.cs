using System;
using System.Diagnostics;
using System.IO;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

public static class ExplorerOpener
{
    public static bool ShouldOpenInExplorer(Repository repository, CommandSetting command)
    {
        // Windows explorer can open both local paths and \\wsl$\... UNC paths.
        if (IsExplorerCommand(command.WindowsLaunchCommand))
        {
            return true;
        }

        return repository.IsWsl && IsExplorerCommand(command.WslLaunchCommand);
    }

    public static bool IsExplorerCommand(string launchCommand)
    {
        if (string.IsNullOrWhiteSpace(launchCommand))
        {
            return false;
        }

        string executableName = Path.GetFileNameWithoutExtension(launchCommand.Trim().Trim('"'));

        return executableName.Equals(Constants.ExplorerCommand, StringComparison.OrdinalIgnoreCase);
    }

    public static void OpenFolder(Repository repository, PluginInitContext context)
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
}
