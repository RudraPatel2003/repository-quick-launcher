using System.Collections.Generic;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

public class RepositoryFinder
{
    public static List<Repository> FindRepositories(Settings? settings)
    {
        List<Repository> repositories = new();

        if (settings == null)
        {
            return repositories;
        }

        Repository test = new(
            "repoitory-quick-launcher",
            "C:\\Users\\patel\\OneDrive\\Desktop\\git\\repository-quick-launcher",
            false
        );

        repositories.Add(test);

        return repositories;
    }
}
