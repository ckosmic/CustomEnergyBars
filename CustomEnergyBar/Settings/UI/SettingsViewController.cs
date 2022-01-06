using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomEnergyBar.Utils;
using System.Reflection;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CustomEnergyBar.Settings.UI
{
	[HotReload(RelativePathToLayout = @"Views\settings.bsml")]
	[ViewDefinition("CustomEnergyBar.Settings.UI.Views.settings.bsml")]
	internal class SettingsViewController : BSMLAutomaticViewController
	{
		private readonly string version = $"{ Assembly.GetExecutingAssembly().GetName().Version.ToString(3) }";
		private const string repoUrl = "https://github.com/ckosmic/CustomEnergyBars";
		private const string wikiUrl = "https://github.com/ckosmic/CustomEnergyBars/wiki";
		private const string releaseUrl = "https://github.com/ckosmic/CustomEnergyBars/releases/latest";

		private EnergyLoader _energyLoader;

		public EnergyBarListViewController energyBarListController;

		[UIComponent("update-message")]
		public TextMeshProUGUI updateMessage;

		[UIComponent("update-button")]
		public Button updateButton;

		[UIValue("allow-sfx")]
		public bool AllowSFX { 
			get {
				return Plugin.Settings.AllowSFX;
			}
			set {
				Plugin.Settings.AllowSFX = value;
				ApplySettings();
			}
		}

		[Inject]
		internal void Construct(EnergyLoader energyLoader) {
			_energyLoader = energyLoader;
		}

		[UIAction("#post-parse")]
		private async void CheckForUpdates() {
			updateMessage.text = "Checking...";
			updateMessage.color = Color.gray;
			updateButton.interactable = false;

			UpdateChecker.Release release = await UpdateChecker.GetNewestReleaseAsync(CancellationToken.None);
			if (release != null) {
				int status = UpdateChecker.CompareRelease(release);
				if (status < 0) {
					// GitHub version is lower than local version, the heck how
					Plugin.Log.Info("Plugin version is higher than on GitHub???");
					updateMessage.text = "Higher version";
				} else if (status == 0) {
					// GitHub version is the same as local version
					updateMessage.text = "Up to date";
				} else {
					// GitHub version is higher than local version and plugin needs to be updated
					Plugin.Log.Info("Plugin needs to be updated.");
					updateMessage.text = "Update available";
					updateMessage.color = Color.green;
					updateButton.interactable = true;
				}
			} else {
				// Failed to get release information
				Plugin.Log.Info("Failed to retrieve release information.");
				updateMessage.text = "Update check failed";
				updateMessage.color = Color.red;
			}
		}

		[UIAction("openGitHubRepo")]
		private void OpenGitHubRepoClicked() {
			Application.OpenURL(repoUrl);
		}

		[UIAction("openWiki")]
		private void OpenWikiClicked() {
			Application.OpenURL(wikiUrl);
		}

		[UIAction("openReleases")]
		private void OpenReleasesClicked() {
			Application.OpenURL(releaseUrl);
		}

		private void ApplySettings() {
			if (energyBarListController != null && energyBarListController._previewGo != null) {
				energyBarListController.ClearEnergyBar();
				energyBarListController.GeneratePreview(_energyLoader.SelectedEnergyBar);
			}
		}
	}
}
