using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.UI;
using FuzzyScore.Net;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher;

public class RepositoryQuickLauncher : IPlugin, ISettingProvider, IReloadable, IContextMenu
{
    private PluginInitContext? _context;
    private Settings? _settings;
    private List<Repository> _repositories = new();

    public void Init(PluginInitContext context)
    {
        _context = context;
        _settings = _context.API.LoadSettingJsonStorage<Settings>();

        if (_settings.EnsureDefaultCommandSetting())
        {
            _context.API.SaveSettingJsonStorage<Settings>();
        }

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
            return Messages.GetMissingDirectoriesMessage(_context);
        }

        CommandSetting? defaultCommand = _settings.GetDefaultCommandSetting();

        if (
            defaultCommand is null
            || string.IsNullOrWhiteSpace(defaultCommand.WindowsLaunchCommand)
            || string.IsNullOrWhiteSpace(defaultCommand.WslLaunchCommand)
        )
        {
            return Messages.GetMissingLaunchCommandsMessage(_context);
        }

        return GetResults(query.Search, defaultCommand);
    }

    public Control CreateSettingPanel()
    {
        SettingsViewModel settingsViewModel = new(_settings!);

        return new SettingsView(_context!, settingsViewModel, ReloadData);
    }

    public void ReloadData()
    {
        if (_context is null)
        {
            return;
        }

        Init(_context);
    }

    public List<Result> LoadContextMenus(Result selectedResult)
    {
        if (_context is null || _settings is null)
        {
            return new List<Result>();
        }

        if (selectedResult.ContextData is not Repository repository)
        {
            return new List<Result>();
        }

        return _settings
            .CommandSettings.Select(commandSetting => new Result
            {
                Title = $"Open in {commandSetting.Name}",
                SubTitle = repository.IsWsl
                    ? $"{commandSetting.WslDistributionName}: {commandSetting.WslLaunchCommand}"
                    : commandSetting.WindowsLaunchCommand,
                IcoPath = Constants.IconPath,
                Action = _ =>
                {
                    RepositoryOpener.OpenFolder(repository, _context, commandSetting);
                    return true;
                },
            })
            .ToList();
    }

    private List<Result> GetResults(string queryString, CommandSetting defaultCommand)
    {
        queryString = queryString.ToLowerInvariant().Trim();

        if (_context is null || _settings is null)
        {
            return new List<Result>();
        }

        IEnumerable<ScoredRepository> scoredRepositoriesQuery = _repositories.Select(
            repository => new ScoredRepository(
                repository,
                FuzzyScorer.Score(repository.Name, queryString)
            )
        );

        if (!string.IsNullOrWhiteSpace(queryString))
        {
            scoredRepositoriesQuery = scoredRepositoriesQuery.Where(repository =>
                repository.Score > 0
            );
        }

        List<ScoredRepository> scoredRepositories = scoredRepositoriesQuery
            .OrderByDescending(repository => repository.Score)
            .ToList();

        if (scoredRepositories.Count == 0)
        {
            return Messages.GetNoResultsMessage(_context);
        }

        List<Result> results = scoredRepositories
            .Select(scoredRepository =>
            {
                Repository repository = scoredRepository.Repository;

                return new Result()
                {
                    Title = repository.GetResultTitle(),
                    SubTitle = repository.GetResultSubTitle(),
                    Score = scoredRepository.Score,
                    IcoPath = Constants.IconPath,
                    ContextData = repository,
                    Action = _ =>
                    {
                        RepositoryOpener.OpenFolder(repository, _context, defaultCommand);
                        return true;
                    },
                };
            })
            .ToList();

        return results;
    }
}
