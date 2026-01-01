<h1 align="center">
  <br>
    <img src="Flow.Launcher.Plugin.RepositoryQuickLauncher/Assets/repository-banner.png" alt="Repository Banner" width="25%">  
  <br>
    Repository Quick Launcher
</h1>

## Description

A plugin for the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher) that allows you to quickly open codebases using your IDE of choice (VSCode, Cursor, Zed, etc.).

## Usage

1. Define the locations of folders that contain codebases in the plugin settings. These can be directories in Windows or in WSL.

    Windows example: `C:\Users\patel\OneDrive\Desktop\git`

    WSL example: `/home/wsl/git`

2. Define the launch command for your IDE of choice.

    Windows example: `code`

    WSL example: `cursor`

Then, type `c` (or your chosen action keyword) and begin typing to fuzzy search for a repository.

## Development

### Setup

Ensure you have `.NET 10` installed on your machine.

You must have a Windows machine with the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher) installed.

### Scripts

To run the plugin locally, make your changes and then run the `debug.ps1` script. This will relaunch the Flow launcher with the plugin loaded.

To generate a release build, run the `release.ps1` script and find it under the `Flow.Launcher.Plugin.RepositoryQuickLauncher/bin/` directory.
