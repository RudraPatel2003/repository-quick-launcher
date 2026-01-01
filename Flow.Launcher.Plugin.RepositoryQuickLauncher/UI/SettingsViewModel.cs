using System.Collections.ObjectModel;
using System.Linq;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.UI;

public class SettingsViewModel : BaseModel
{
    public Settings Settings { get; }

    public string WindowsLaunchCommand
    {
        get => Settings.WindowsLaunchCommand;
        set
        {
            Settings.WindowsLaunchCommand = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> WindowsDirectoriesCollection { get; }

    public string WslDistributionName
    {
        get => Settings.WslDistributionName;
        set
        {
            Settings.WslDistributionName = value;
            OnPropertyChanged();
        }
    }

    public string WslLaunchCommand
    {
        get => Settings.WslLaunchCommand;
        set
        {
            Settings.WslLaunchCommand = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> WslDirectoriesCollection { get; }

    public SettingsViewModel(Settings settings)
    {
        Settings = settings;
        WindowsDirectoriesCollection = new ObservableCollection<string>(
            Settings.WindowsDirectories
        );
        WslDirectoriesCollection = new ObservableCollection<string>(Settings.WslDirectories);
    }

    public void SyncDirectoriesToSettings()
    {
        Settings.WindowsDirectories = WindowsDirectoriesCollection
            .Select(u => u.Trim())
            .Where(u => u.Length > 0)
            .ToList();

        Settings.WslDirectories = WslDirectoriesCollection
            .Select(u => u.Trim())
            .Where(u => u.Length > 0)
            .ToList();
    }
}
