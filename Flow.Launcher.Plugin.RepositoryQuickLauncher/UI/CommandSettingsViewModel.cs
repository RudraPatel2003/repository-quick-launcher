using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Helpers;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.UI;

public class CommandSettingsViewModel : BaseModel
{
    public Settings Settings { get; }

    public ObservableCollection<CommandSettingRowViewModel> CommandSettings { get; }

    public bool CanDelete => CommandSettings.Count > 1;

    public bool CanSave => HasUnsavedChanges();

    public CommandSettingsViewModel(Settings settings)
    {
        Settings = settings;
        CommandSettings = new ObservableCollection<CommandSettingRowViewModel>(
            settings.CommandSettings.Select(CommandSettingRowViewModel.FromModel)
        );

        foreach (CommandSettingRowViewModel row in CommandSettings)
        {
            SubscribeToRow(row);
        }

        CommandSettings.CollectionChanged += OnCommandSettingsCollectionChanged;
    }

    private void OnCommandSettingsCollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        OnPropertyChanged(nameof(CanDelete));
        OnPropertyChanged(nameof(CanSave));

        if (e.NewItems is not null)
        {
            foreach (CommandSettingRowViewModel row in e.NewItems)
            {
                SubscribeToRow(row);
            }
        }

        if (e.OldItems is not null)
        {
            foreach (CommandSettingRowViewModel row in e.OldItems)
            {
                UnsubscribeFromRow(row);
            }
        }
    }

    private void SubscribeToRow(CommandSettingRowViewModel row)
    {
        row.PropertyChanged += OnRowPropertyChanged;
    }

    private void UnsubscribeFromRow(CommandSettingRowViewModel row)
    {
        row.PropertyChanged -= OnRowPropertyChanged;
    }

    private void OnRowPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CanSave));
    }

    public void AddRow()
    {
        CommandSettings.Add(
            new CommandSettingRowViewModel
            {
                Name = "New Command",
                WindowsLaunchCommand = Constants.CodeCommand,
                WslDistributionName = "Ubuntu",
                WslLaunchCommand = Constants.CodeCommand,
                IsDefault = CommandSettings.Count == 0,
            }
        );
    }

    public bool TryDeleteSelected(IEnumerable<CommandSettingRowViewModel> selectedRows)
    {
        if (CommandSettings.Count <= 1)
        {
            return false;
        }

        List<CommandSettingRowViewModel> rowsToDelete = selectedRows.ToList();

        if (rowsToDelete.Count == 0)
        {
            return false;
        }

        if (CommandSettings.Count - rowsToDelete.Count < 1)
        {
            return false;
        }

        bool deletedDefault = rowsToDelete.Any(row => row.IsDefault);

        foreach (CommandSettingRowViewModel row in rowsToDelete)
        {
            _ = CommandSettings.Remove(row);
        }

        if (deletedDefault && CommandSettings.Count > 0)
        {
            SetDefault(CommandSettings[0]);
        }

        return true;
    }

    public void SetDefault(CommandSettingRowViewModel selectedRow)
    {
        foreach (CommandSettingRowViewModel row in CommandSettings)
        {
            row.IsDefault = ReferenceEquals(row, selectedRow);
        }
    }

    public string? Validate()
    {
        if (CommandSettings.Count == 0)
        {
            return "At least one command setting is required.";
        }

        int defaultCount = CommandSettings.Count(row => row.IsDefault);

        if (defaultCount != 1)
        {
            return "Exactly one command setting must be marked as default.";
        }

        foreach (CommandSettingRowViewModel row in CommandSettings)
        {
            if (string.IsNullOrWhiteSpace(row.Name))
            {
                return "Each command setting must have a name.";
            }

            if (string.IsNullOrWhiteSpace(row.WindowsLaunchCommand))
            {
                return $"Windows launch command is required for \"{row.Name}\".";
            }

            if (string.IsNullOrWhiteSpace(row.WslDistributionName))
            {
                return $"WSL distribution name is required for \"{row.Name}\".";
            }

            if (string.IsNullOrWhiteSpace(row.WslLaunchCommand))
            {
                return $"WSL launch command is required for \"{row.Name}\".";
            }
        }

        return null;
    }

    public void SyncToSettings()
    {
        Settings.CommandSettings = CommandSettings.Select(row => row.ToModel()).ToList();
        OnPropertyChanged(nameof(CanSave));
    }

    private bool HasUnsavedChanges()
    {
        return !CommandSettings
            .Select(row => row.ToModel())
            .SequenceEqual(Settings.CommandSettings);
    }
}
