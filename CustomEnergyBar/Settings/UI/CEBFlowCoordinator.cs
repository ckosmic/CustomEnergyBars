using BeatSaberMarkupLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMUI;
using Zenject;

namespace CustomEnergyBar.Settings.UI
{
	internal class CEBFlowCoordinator : FlowCoordinator
	{

		private EnergyBarListViewController _energyBarListViewController;
		private EnergyBarPreviewViewController _energyBarPreviewViewController;
		private SettingsViewController _settingsViewController;

		[Inject]
		internal void Construct(EnergyBarListViewController energyBarListViewController, EnergyBarPreviewViewController energyBarPreviewViewController, SettingsViewController settingsViewController) {
			_energyBarListViewController = energyBarListViewController;
			_energyBarPreviewViewController = energyBarPreviewViewController;
			_settingsViewController = settingsViewController;

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
