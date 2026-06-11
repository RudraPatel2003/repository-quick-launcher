using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.UI;

public partial class SettingsView : UserControl
{
    private readonly PluginInitContext _context;
    private readonly SettingsViewModel _viewModel;
    private readonly Action _reloadData;

    public SettingsView(PluginInitContext context, SettingsViewModel viewModel, Action reloadData)
    {
        _context = context;
        _viewModel = viewModel;
        _reloadData = reloadData;

        DataContext = viewModel;

        InitializeComponent();

        CommandSettingsPanelHost.Content = new CommandSettingsView(
            context,
            viewModel.CommandSettingsViewModel,
            reloadData
        );
    }

    private void Save()
    {
        _viewModel.SyncDirectoriesToSettings();
        _context.API.SaveSettingJsonStorage<Settings>();
        _reloadData();
    }

    private void WindowsDirectoriesButtonAddClick(object sender, RoutedEventArgs e)
    {
        string newDirectory = WindowsDirectoriesInput.Text.Trim();

        WindowsDirectoriesInput.Clear();

        if (string.IsNullOrWhiteSpace(newDirectory))
        {
            return;
        }

        if (
            _viewModel.WindowsDirectoriesCollection.Contains(
                newDirectory,
                StringComparer.OrdinalIgnoreCase
            )
        )
        {
            return;
        }

        _viewModel.WindowsDirectoriesCollection.Add(newDirectory);

        Save();
    }

    private void WindowsDirectoriesButtonDeleteClick(object sender, RoutedEventArgs e)
    {
        foreach (string? selected in WindowsDirectoriesList.SelectedItems.Cast<string>().ToList())
        {
            _ = _viewModel.WindowsDirectoriesCollection.Remove(selected);
        }

        Save();
    }

    private void WslDirectoriesButtonAddClick(object sender, RoutedEventArgs e)
    {
        string newDirectory = WslDirectoriesInput.Text.Trim();

        WslDirectoriesInput.Clear();

        if (string.IsNullOrWhiteSpace(newDirectory))
        {
            return;
        }

        if (
            _viewModel.WslDirectoriesCollection.Contains(
                newDirectory,
                StringComparer.OrdinalIgnoreCase
            )
        )
        {
            return;
        }

        _viewModel.WslDirectoriesCollection.Add(newDirectory);

        Save();
    }

    private void WslDirectoriesButtonDeleteClick(object sender, RoutedEventArgs e)
    {
        foreach (string? selected in WslDirectoriesList.SelectedItems.Cast<string>().ToList())
        {
            _ = _viewModel.WslDirectoriesCollection.Remove(selected);
        }

        Save();
    }
}
