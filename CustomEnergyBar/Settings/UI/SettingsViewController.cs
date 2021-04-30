using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar.Settings.UI
{
	internal class SettingsViewController : BSMLResourceViewController
	{
		public override string ResourceName => "CustomEnergyBar.Settings.UI.Views.settings.bsml";

		public EnergyBarListViewController energyBarListController;

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

		private void ApplySettings() {
			if (energyBarListController != null && energyBarListController._previewGo != null) {
				energyBarListController.ClearEnergyBar();
				energyBarListController.GeneratePreview(EnergyLoader.SelectedEnergyBar);
			}
		}
	}
}
