using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.UI;

public partial class CommandSettingsView : UserControl
{
    private readonly PluginInitContext _context;
    private readonly CommandSettingsViewModel _viewModel;
    private readonly Action _reloadData;

    public CommandSettingsView(
        PluginInitContext context,
        CommandSettingsViewModel viewModel,
        Action reloadData
    )
    {
        _context = context;
        _viewModel = viewModel;
        _reloadData = reloadData;

        DataContext = viewModel;

        InitializeComponent();
    }

    private void AddButtonClick(object sender, RoutedEventArgs e)
    {
        _viewModel.AddRow();
    }

    private void DeleteButtonClick(object sender, RoutedEventArgs e)
    {
        if (!_viewModel.CanDelete)
        {
            return;
        }

        _ = _viewModel.TryDeleteSelected(
            CommandSettingsGrid.SelectedItems.Cast<CommandSettingRowViewModel>()
        );
    }

    private void SaveButtonClick(object sender, RoutedEventArgs e)
    {
        string? validationError = _viewModel.Validate();

        if (validationError is not null)
        {
            _context.API.ShowMsg("Invalid command settings", validationError);
            return;
        }

        _viewModel.SyncToSettings();

        _context.API.SaveSettingJsonStorage<Settings>();

        _reloadData();
    }

    private void IsDefaultCheckBoxChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox checkBox)
        {
            return;
        }

        if (checkBox.DataContext is not CommandSettingRowViewModel row || !row.IsDefault)
        {
            return;
        }

        _viewModel.SetDefault(row);
    }
}
