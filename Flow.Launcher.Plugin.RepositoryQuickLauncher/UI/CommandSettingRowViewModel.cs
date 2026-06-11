using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.UI;

public class CommandSettingRowViewModel : BaseModel
{
    private string _name = string.Empty;
    private string _windowsLaunchCommand = string.Empty;
    private string _wslDistributionName = string.Empty;
    private string _wslLaunchCommand = string.Empty;
    private bool _isDefault;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string WindowsLaunchCommand
    {
        get => _windowsLaunchCommand;
        set
        {
            _windowsLaunchCommand = value;
            OnPropertyChanged();
        }
    }

    public string WslDistributionName
    {
        get => _wslDistributionName;
        set
        {
            _wslDistributionName = value;
            OnPropertyChanged();
        }
    }

    public string WslLaunchCommand
    {
        get => _wslLaunchCommand;
        set
        {
            _wslLaunchCommand = value;
            OnPropertyChanged();
        }
    }

    public bool IsDefault
    {
        get => _isDefault;
        set
        {
            _isDefault = value;
            OnPropertyChanged();
        }
    }

    public static CommandSettingRowViewModel FromModel(CommandSetting commandSetting)
    {
        return new CommandSettingRowViewModel
        {
            Name = commandSetting.Name,
            WindowsLaunchCommand = commandSetting.WindowsLaunchCommand,
            WslDistributionName = commandSetting.WslDistributionName,
            WslLaunchCommand = commandSetting.WslLaunchCommand,
            IsDefault = commandSetting.IsDefault,
        };
    }

    public CommandSetting ToModel()
    {
        return new CommandSetting
        {
            Name = Name.Trim(),
            WindowsLaunchCommand = WindowsLaunchCommand.Trim(),
            WslDistributionName = WslDistributionName.Trim(),
            WslLaunchCommand = WslLaunchCommand.Trim(),
            IsDefault = IsDefault,
        };
    }
}
