using System;
using System.Collections.Generic;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public class CommandSetting : IEquatable<CommandSetting>
{
    public string Name { get; set; } = "VS Code";

    public string WindowsLaunchCommand { get; set; } = Constants.CodeCommand;

    public string WslDistributionName { get; set; } = Constants.DefaultWslDistribution;

    public string WslLaunchCommand { get; set; } = Constants.CodeCommand;

    public bool IsDefault { get; set; }

    public static List<CommandSetting> CreateInitialCommandSettings()
    {
        return new List<CommandSetting> { CreateVscodeCommand(), CreateFileExplorerCommand() };
    }

    public bool Equals(CommandSetting? other)
    {
        if (other is null)
        {
            return false;
        }

        bool isNameEqual = string.Equals(Name.Trim(), other.Name.Trim(), StringComparison.Ordinal);

        bool isWindowsLaunchCommandEqual = string.Equals(
            WindowsLaunchCommand.Trim(),
            other.WindowsLaunchCommand.Trim(),
            StringComparison.Ordinal
        );

        bool isWslDistributionNameEqual = string.Equals(
            WslDistributionName.Trim(),
            other.WslDistributionName.Trim(),
            StringComparison.Ordinal
        );

        bool isWslLaunchCommandEqual = string.Equals(
            WslLaunchCommand.Trim(),
            other.WslLaunchCommand.Trim(),
            StringComparison.Ordinal
        );

        bool isDefaultEqual = IsDefault == other.IsDefault;

        return isNameEqual
            && isWindowsLaunchCommandEqual
            && isWslDistributionNameEqual
            && isWslLaunchCommandEqual
            && isDefaultEqual;
    }

    public override bool Equals(object? obj)
    {
        return obj is CommandSetting other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Name.Trim(),
            WindowsLaunchCommand.Trim(),
            WslDistributionName.Trim(),
            WslLaunchCommand.Trim(),
            IsDefault
        );
    }

    private static CommandSetting CreateVscodeCommand()
    {
        return new CommandSetting
        {
            Name = "VS Code",
            WindowsLaunchCommand = Constants.CodeCommand,
            WslDistributionName = Constants.DefaultWslDistribution,
            WslLaunchCommand = Constants.CodeCommand,
            IsDefault = true,
        };
    }

    private static CommandSetting CreateFileExplorerCommand()
    {
        return new CommandSetting
        {
            Name = "File Explorer",
            WindowsLaunchCommand = Constants.ExplorerCommand,
            WslDistributionName = Constants.DefaultWslDistribution,
            WslLaunchCommand = Constants.ExplorerExecutable,
            IsDefault = false,
        };
    }
}
