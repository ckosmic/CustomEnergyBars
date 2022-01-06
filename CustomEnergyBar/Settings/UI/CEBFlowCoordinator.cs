using BeatSaberMarkupLanguage;
using System;
using HMUI;
using Zenject;

namespace CustomEnergyBar.Settings.UI
{
	internal class CEBFlowCoordinator : FlowCoordinator
	{

		private EnergyBarListViewController _energyBarListViewController;
		private EnergyBarPreviewViewController _energyBarPreviewViewController;
		private SettingsViewController _settingsViewController;
		private EnergyLoader _energyLoader;

		[Inject]
		internal void Construct(EnergyBarListViewController energyBarListViewController, EnergyBarPreviewViewController energyBarPreviewViewController, SettingsViewController settingsViewController, EnergyLoader energyLoader) {
			_energyBarListViewController = energyBarListViewController;
			_energyBarPreviewViewController = energyBarPreviewViewController;
			_settingsViewController = settingsViewController;
			_energyLoader = energyLoader;

			_settingsViewController.energyBarListController = _energyBarListViewController;
			_energyBarListViewController.energyBarPreviewViewController = _energyBarPreviewViewController;
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
			try {
				if (firstActivation) {
					_energyLoader.Load();

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
