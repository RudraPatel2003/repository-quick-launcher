using System.Collections.Generic;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public class Settings
{
    public string WindowsLaunchCommand { get; set; } = "code";
    public List<string> WindowsDirectories { get; set; } = new List<string>();

    public string WslDistributionName { get; set; } = "Ubuntu";
    public string WslLaunchCommand { get; set; } = "code";
    public List<string> WslDirectories { get; set; } = new List<string>();
}
