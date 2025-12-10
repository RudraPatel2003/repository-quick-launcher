using System;
using System.Collections.Generic;
using System.IO;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;

public class RepositoryFinder
{
    public static List<Repository> FindRepositories(Settings? settings, PluginInitContext context)
    {
        List<Repository> repositories = new();

        if (settings == null)
        {
            return repositories;
        }

        List<Repository> windowsRepositories = GetWindowsRepositories(settings, context);
        List<Repository> wslRepositories = GetWslRepositories(settings, context);

        repositories.AddRange(windowsRepositories);
        repositories.AddRange(wslRepositories);

        return repositories;
    }

    public static List<Repository> GetWindowsRepositories(
        Settings settings,
        PluginInitContext context
    )
    {
        List<Repository> repositories = new();

        List<string> windowsDirectories = settings.WindowsDirectories;

        foreach (string directory in windowsDirectories)
        {
            List<string> paths = GetImmediateDirectories(directory, context);

            foreach (string path in paths)
            {
                repositories.Add(new Repository(path, directory));
            }
        }

        return repositories;
    }

    public static List<Repository> GetWslRepositories(Settings settings, PluginInitContext context)
    {
        List<Repository> repositories = new();

        string wslDistributionName = settings.WslDistributionName;
        string partialWslPath = Constants.WslPrefix + wslDistributionName;

        List<string> wslDirectories = settings.WslDirectories;

        foreach (string directory in wslDirectories)
        {
            string wslUncDirectoryName = partialWslPath + directory;

            List<string> paths = GetImmediateDirectories(wslUncDirectoryName, context);

            foreach (string path in paths)
            {
                repositories.Add(new Repository(path, directory));
            }
        }

        return repositories;
    }

    private static List<string> GetImmediateDirectories(string directory, PluginInitContext context)
    {
        List<string> paths = new();

        if (!Directory.Exists(directory))
        {
            context.API.ShowMsg(
                "Directory provided to RepositoryQuickLauncher does not exist",
                directory
            );
            return paths;
        }

        try
        {
            paths.AddRange(Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly));
        }
        catch (Exception ex)
        {
            context.API.ShowMsg($"Error reading directory {directory}", ex.Message);
        }

        return paths;
    }
}
