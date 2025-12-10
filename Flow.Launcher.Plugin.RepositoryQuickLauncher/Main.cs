using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.UI;
using FuzzyScore.Net;

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
        _repositories = RepositoryFinder.FindRepositories(_settings, _context);
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

        LauncherType launcher = LauncherParser.GetLauncher(queryWords[0]);
        string queryString = string.Join(" ", queryWords[1..]);

        if (launcher == LauncherType.Invalid)
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

    private List<Result> GetResults(LauncherType launcher, string queryString)
    {
        queryString = queryString.ToLowerInvariant().Trim();

        if (_context is null || _settings is null)
        {
            return new List<Result>();
        }

        List<ScoredRepository> scoredRepositories = _repositories
            .Select(repository => new ScoredRepository(
                repository,
                FuzzyScorer.Score(repository.Name, queryString)
            ))
            .Where(repository => repository.Score > 0)
            .OrderByDescending(repository => repository.Score)
            .ToList();

        if (scoredRepositories.Count == 0)
        {
            return Messages.GetNoResultsMessage(_context);
        }

        List<Result> results = scoredRepositories
            .Select(scoredRepository => new Result()
            {
                Title = scoredRepository.Repository.GetResultTitle(),
                SubTitle = scoredRepository.Repository.GetResultSubTitle(),
                Score = scoredRepository.Score,
                IcoPath = Constants.IconPath,
                Action = (e) =>
                {
                    RepositoryOpener.OpenFolder(
                        launcher,
                        scoredRepository.Repository,
                        _context,
                        _settings
                    );
                    return true;
                },
            })
            .ToList();

        return results;
    }
}
