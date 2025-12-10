using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.UI;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher;

public class RepositoryQuickLauncher : IPlugin, ISettingProvider, IReloadable
{
    private PluginInitContext? _context;
    private Settings? _settings;
    private List<Repository> _repositories = new();

    public void Init(PluginInitContext context)
    {
        _context = context;
        _settings = _context.API.LoadSettingJsonStorage<Settings>();
        _repositories = RepositoryFinder.FindRepositories(_settings);
    }

    public List<Result> Query(Query query)
    {
        if (_context is null || _settings is null)
        {
            return Messages.GetLoadingMessage();
        }

        if (_settings.WindowsDirectories.Count == 0 && _settings.WslDirectories.Count == 0)
        {
            return Messages.GetMissingConfigurationMessage(_context);
        }

        string[] queryWords = query.Search.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
        );

        if (queryWords.Length is 0 or 1)
        {
            return Messages.GetEnterCommandMessage();
        }

        LauncherEnumeration launcher = LauncherExtensions.GetLauncher(queryWords[0]);
        string queryString = string.Join(" ", queryWords[1..]);

        if (launcher == LauncherEnumeration.Invalid)
        {
            return Messages.GetInvalidCommandMessage();
        }

        return GetResults(launcher, queryString);
    }

    public Control CreateSettingPanel()
    {
        SettingsViewModel settingsViewModel = new(_settings ?? new Settings());

        return new SettingsView(_context!, settingsViewModel);
    }

    public void ReloadData()
    {
        if (_context is null)
        {
            return;
        }

        Init(_context);
    }

    private List<Result> GetResults(LauncherEnumeration launcher, string queryString)
    {
        List<Result> results = new();

        Result test = new()
        {
            Title = LauncherExtensions.GetLauncherCommand(launcher) + " " + _repositories[0].Name,
            SubTitle = queryString,
            IcoPath = Constants.IconPath,
        };

        results.Add(test);

        return results;
    }
}
