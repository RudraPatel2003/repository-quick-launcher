<h1 align="center">
  <br>
    <img src="Flow.Launcher.Plugin.RepositoryQuickLauncher/Assets/repository-banner.png" alt="Repository Banner" width="25%">  
  <br>
    Repository Quick Launcher
</h1>

## Description

A plugin for the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher) that allows you to quickly open codebases using VS Code or Cursor.

## Usage

Define the locations of folders that contain codebases in the plugin settings. These can be directories in Windows or in WSL.

Windows example: `C:\Users\patel\OneDrive\Desktop\git`

WSL example: `/home/wsl/git`

Then, type `repo` followed by `code` or `cursor` to fuzzy search for codebases and open them in VS Code or Cursor respectively.

## Getting Started

### Setup

Ensure you have `.NET 10` installed on your machine.

You must have a Windows machine with the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher) installed.

### Local Development

To run the plugin locally, make your changes and then run the `debug.ps1` script. This will relaunch the Flow launcher with the plugin loaded.

To generate a release build, run the `release.ps1` script and find it under the `Flow.Launcher.Plugin.RepositoryQuickLauncher/bin/` directory.
