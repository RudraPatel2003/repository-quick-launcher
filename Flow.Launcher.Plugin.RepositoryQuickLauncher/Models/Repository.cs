namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public class Repository
{
    public string Name { get; set; }
    public string OriginalPath { get; set; }
    public string NormalizedPath { get; set; }
    public bool IsWsl { get; set; }

    public Repository(string name, string originalPath, bool isWsl)
    {
        Name = name;
        OriginalPath = originalPath;
        IsWsl = isWsl;

        NormalizedPath = originalPath;
    }
}
