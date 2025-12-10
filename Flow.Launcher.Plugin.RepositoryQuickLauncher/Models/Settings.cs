using System.Collections.Generic;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public class Settings
{
    public List<string> WindowsDirectories { get; set; } = new List<string>();
    public string WslDistributionName { get; set; } = "Ubuntu";
    public List<string> WslDirectories { get; set; } = new List<string>();
}
