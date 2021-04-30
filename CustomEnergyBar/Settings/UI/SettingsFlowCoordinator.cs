using BeatSaberMarkupLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMUI;

namespace CustomEnergyBar.Settings.UI
{
	internal class SettingsFlowCoordinator : FlowCoordinator
	{

		private EnergyBarListViewController _energyBarListViewController;
		private EnergyBarPreviewViewController _energyBarPreviewViewController;
		private SettingsViewController _settingsViewController;

		public void Awake() {
			if (!_energyBarListViewController) {
				_energyBarListViewController = BeatSaberUI.CreateViewController<EnergyBarListViewController>();
			}
			if (!_energyBarPreviewViewController) {
				_energyBarPreviewViewController = BeatSaberUI.CreateViewController<EnergyBarPreviewViewController>();
			}
			if (!_settingsViewController) {
				_settingsViewController = BeatSaberUI.CreateViewController<SettingsViewController>();
			}
			_settingsViewController.energyBarListController = _energyBarListViewController;
			_energyBarListViewController.energyBarPreviewViewController = _energyBarPreviewViewController;
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
			try {
				if (firstActivation) {
					SetTitle("Custom Energy Bars", ViewController.AnimationType.In);
					showBackButton = true;

					ProvideInitialViewControllers(_energyBarListViewController, _settingsViewController, _energyBarPreviewViewController, null, null);
				}
			} catch (Exception e) {
				Plugin.Log.Error(e);
			}
		}

		protected override void BackButtonWasPressed(ViewController topViewController) {
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal, false);
		}

	}
}
