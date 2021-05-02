# CustomEnergyBars
A Beat Saber mod that adds custom energy bars to the game.

## Installation
* Install BS Utils and BSML via ModAssistant or manually
* Download the [latest release](https://github.com/ckosmic/CustomEnergyBars/releases/latest) from the releases page
* Drop the file into the Plugins folder in your Beat Saber directory

To install custom energy bars, place a `.energy` file into the CustomEnergyBars folder located in your Beat Saber directory.  If this folder doesn't exist, either create it yourself or run Beat Saber at least once with the mod installed.

## Creating your own Custom Energy Bar
Refer to the [wiki](https://github.com/ckosmic/CustomEnergyBars/wiki) to learn how to make a custom energy bar.

## For Developers
### Contributing to CustomEnergyBars (taken from [CustomSabers](https://github.com/nalulululuna/CustomSaberPlugin))
In order to build this project, please create the file CustomEnergyBar.csproj.user and add your Beat Saber directory path to it in the project directory. This file should not be uploaded to GitHub and is in the .gitignore.

```xml 
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <!-- Set "YOUR OWN" Beat Saber folder here to resolve most of the dependency paths! -->
    <BeatSaberDir>E:\Program Files (x86)\Steam\steamapps\common\Beat Saber</BeatSaberDir>
  </PropertyGroup>
</Project>
```

If you plan on adding any new dependencies which are located in the Beat Saber directory, it would be nice if you edited the paths to use $(BeatSaberDir) in CustomEnergyBar.csproj.

```xml 
...
<Reference Include="BS_Utils">
  <HintPath>$(BeatSaberDir)\Plugins\BS_Utils.dll</HintPath>
</Reference>
<Reference Include="IPA.Loader">
  <HintPath>$(BeatSaberDir)\Beat Saber_Data\Managed\IPA.Loader.dll</HintPath>
</Reference>
...
```
