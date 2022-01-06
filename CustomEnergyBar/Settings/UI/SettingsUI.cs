using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace CustomEnergyBar.Settings.UI
{
	internal class SettingsUI : IInitializable, IDisposable
	{
		private readonly CEBFlowCoordinator _flowCoordinator;

		private MenuButton _menuButton;

		public static bool buttonCreated = false;

		public SettingsUI(CEBFlowCoordinator flowCoordinator) {
			_flowCoordinator = flowCoordinator;

			_menuButton = new MenuButton("Custom Energy Bars", "Choose custom energy bars here!", OnMenuButtonWasPressed, true);
			
		}

		public void Initialize() {
			PersistentSingleton<MenuButtons>.instance.RegisterButton(_menuButton);
			buttonCreated = true;
		}

		public void Dispose() {
			if (_menuButton == null) return;
			if (MenuButtons.IsSingletonAvailable && BSMLParser.IsSingletonAvailable) {
				PersistentSingleton<MenuButtons>.instance.UnregisterButton(_menuButton);
			}
			_menuButton = null;
		}

		private void OnMenuButtonWasPressed() {
			if (_flowCoordinator == null) return;
			BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(_flowCoordinator);
		}

	}
}
