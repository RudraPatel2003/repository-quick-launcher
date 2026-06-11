using System.Collections.Generic;
using System.Linq;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public class Settings
{
    public List<string> WindowsDirectories { get; set; } = new List<string>();

    public List<string> WslDirectories { get; set; } = new List<string>();

    public List<CommandSetting> CommandSettings { get; set; } =
        CommandSetting.CreateInitialCommandSettings();

    public CommandSetting? GetDefaultCommandSetting()
    {
        return CommandSettings.FirstOrDefault(command => command.IsDefault);
    }

    public bool EnsureDefaultCommandSetting()
    {
        if (CommandSettings.Count > 0)
        {
            if (CommandSettings.All(command => !command.IsDefault))
            {
                CommandSettings[0].IsDefault = true;
            }

            return false;
        }

        CommandSettings.AddRange(CommandSetting.CreateInitialCommandSettings());
        return true;
    }
}
