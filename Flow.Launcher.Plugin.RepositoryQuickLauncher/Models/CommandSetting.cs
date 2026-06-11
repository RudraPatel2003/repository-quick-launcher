using System;
using System.Collections.Generic;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public class CommandSetting : IEquatable<CommandSetting>
{
    public string Name { get; set; } = "VS Code";

    public string WindowsLaunchCommand { get; set; } = Constants.CodeCommand;

    public string WslDistributionName { get; set; } = "Ubuntu";

    public string WslLaunchCommand { get; set; } = Constants.CodeCommand;

    public bool IsDefault { get; set; }

    public static CommandSetting CreateDefault()
    {
        return new CommandSetting
        {
            Name = "VS Code",
            WindowsLaunchCommand = Constants.CodeCommand,
            WslDistributionName = "Ubuntu",
            WslLaunchCommand = Constants.CodeCommand,
            IsDefault = true,
        };
    }

    public static CommandSetting CreateExplorer()
    {
        return new CommandSetting
        {
            Name = "Explorer",
            WindowsLaunchCommand = Constants.ExplorerCommand,
            WslDistributionName = "Ubuntu",
            WslLaunchCommand = Constants.ExplorerExecutable,
            IsDefault = false,
        };
    }

    public static List<CommandSetting> CreateInitialCommandSettings()
    {
        return new List<CommandSetting> { CreateDefault(), CreateExplorer() };
    }

    public bool Equals(CommandSetting? other)
    {
        if (other is null)
        {
            return false;
        }

        return string.Equals(Name.Trim(), other.Name.Trim(), StringComparison.Ordinal)
            && string.Equals(
                WindowsLaunchCommand.Trim(),
                other.WindowsLaunchCommand.Trim(),
                StringComparison.Ordinal
            )
            && string.Equals(
                WslDistributionName.Trim(),
                other.WslDistributionName.Trim(),
                StringComparison.Ordinal
            )
            && string.Equals(
                WslLaunchCommand.Trim(),
                other.WslLaunchCommand.Trim(),
                StringComparison.Ordinal
            )
            && IsDefault == other.IsDefault;
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
}
