using System;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public class Repository
{
    /// <summary>
    /// Path from <c>Directory.GetDirectories</c>; uses UNC format for WSL; useful for Windows paths
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// WSL path, such as <c>/git/repository-quick-launcher</c>
    /// </summary>
    public string WslPath { get; set; }

    /// <summary>
    /// User supplied directory, such as <c>/git</c>
    /// </summary>
    public string Directory { get; set; }

    /// <summary>
    /// Folder name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Determines whether the repository is in WSL
    /// </summary>
    public bool IsWsl { get; set; }

    public Repository(string path, string directory)
    {
        Path = path;
        Directory = directory;
        Name = GetRepositoryNameFromPath(path);
        WslPath = GetWslPath(directory, Name);
        IsWsl = Directory.StartsWith('/');
    }

    private static string GetRepositoryNameFromPath(string path)
    {
        string directoryName = System.IO.Path.GetFileName(
            path.TrimEnd(
                System.IO.Path.DirectorySeparatorChar,
                System.IO.Path.AltDirectorySeparatorChar
            )
        );

        return directoryName;
    }

    private static string GetWslPath(string directory, string name)
    {
        bool needsTrailingSlash = !directory.EndsWith("/", StringComparison.Ordinal);

        return directory + (needsTrailingSlash ? "/" : "") + name;
    }

    public string GetResultTitle()
    {
        if (IsWsl)
        {
            return $"WSL: {Name}";
        }

        return Name;
    }

    public string GetResultSubTitle()
    {
        if (IsWsl)
        {
            return WslPath;
        }

        return Path;
    }
}
