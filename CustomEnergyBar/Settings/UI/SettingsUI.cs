using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomEnergyBar.Settings.UI
{
	internal class SettingsUI
	{
		public static bool buttonCreated = false;
		public static SettingsFlowCoordinator flowCoordinator;

		public static void CreateButton() {
			if (!buttonCreated) {
				MenuButton menuButton = new MenuButton("Custom Energy Bars", "Choose custom energy bars here!", new Action(OnMenuButtonWasPressed), true);
				PersistentSingleton<MenuButtons>.instance.RegisterButton(menuButton);
				buttonCreated = true;
			}
		}

		public static void CreateFlowCoordinator() {
			if (flowCoordinator == null) {
				flowCoordinator = BeatSaberUI.CreateFlowCoordinator<SettingsFlowCoordinator>();
			}
			BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(flowCoordinator, null, ViewController.AnimationDirection.Horizontal, false, false);
		}

		private static void OnMenuButtonWasPressed() {
			CreateFlowCoordinator();
		}

	}
}
