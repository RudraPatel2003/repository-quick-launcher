using System.Collections.ObjectModel;
using System.Linq;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.UI;

public class SettingsViewModel : BaseModel
{
    public Settings Settings { get; }

    public ObservableCollection<string> WindowsDirectoriesCollection { get; }

    public ObservableCollection<string> WslDirectoriesCollection { get; }

    public CommandSettingsViewModel CommandSettingsViewModel { get; }

    public SettingsViewModel(Settings settings)
    {
        Settings = settings;
        WindowsDirectoriesCollection = new ObservableCollection<string>(
            Settings.WindowsDirectories
        );
        WslDirectoriesCollection = new ObservableCollection<string>(Settings.WslDirectories);
        CommandSettingsViewModel = new CommandSettingsViewModel(Settings);
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
