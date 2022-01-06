using CustomEnergyBar.API;
using HMUI;
using UnityEngine;
using Zenject;

namespace CustomEnergyBar
{
	internal class EnergyBarManager : IInitializable {

		private readonly GameEnergyCounter _gameEnergyCounter;
		private readonly GameEnergyUIPanel _gameEnergyUIPanel;

		public EnergyBarManager(GameEnergyCounter gameEnergyCounter, GameEnergyUIPanel gameEnergyUIPanel) {
			_gameEnergyCounter = gameEnergyCounter;
			_gameEnergyUIPanel = gameEnergyUIPanel;
		}

		public void Initialize() {
			if (Plugin.Settings.Selected != "defaultEnergyBar") {
				EnergyBar energyBar = (CEBAPI.overrideBar != null) ? CEBAPI.overrideBar : EnergyLoader.GetEnergyBarByBundleId(Plugin.Settings.Selected);
				InstantiateEnergyBar(energyBar);
			}
		}

		public void AddManagers(GameObject energyGo) {
			if (energyGo.GetComponentInChildren<EventManager>(true) != null) {
				foreach (EventManager manager in energyGo.GetComponentsInChildren<EventManager>(true)) {
					EnergyEventManager eem = manager.gameObject.AddComponent<EnergyEventManager>();
					eem.Initialize(_gameEnergyCounter, manager, energyGo);
				}
			}
			if (Plugin.Settings.AllowSFX == false && energyGo.GetComponentInChildren<AudioSource>(true) != null) {
				foreach (AudioSource audio in energyGo.GetComponentsInChildren<AudioSource>()) {
					UnityEngine.Object.Destroy(audio);
				}
			}
		}

		public void InstantiateEnergyBar(EnergyBar energyBar) {
			if (_gameEnergyUIPanel.gameObject.activeInHierarchy)
			{
				GameObject prefab = energyBar.energyBarPrefab;
				GameObject go = UnityEngine.Object.Instantiate(prefab, _gameEnergyUIPanel.transform.position, _gameEnergyUIPanel.transform.rotation, _gameEnergyUIPanel.transform.parent);
				if (_gameEnergyUIPanel.transform.parent.parent.name == "FlyingGameHUD")
				{
					// 360 and 90 degree levels need the bar to be scaled up
					go.transform.localScale *= 50;
				}
				AddManagers(go);
				EnergyBarDescriptor descriptor = go.GetComponent<EnergyBarDescriptor>();
				switch (_gameEnergyCounter.energyType)
				{
					case GameplayModifiers.EnergyType.Bar:
						descriptor.standardBar.gameObject.SetActive(true);
						break;
					case GameplayModifiers.EnergyType.Battery:
						int lives = _gameEnergyCounter.batteryLives;
						GameObject batteryBar = descriptor.GetBatteryBar(lives);
						descriptor.standardBar.gameObject.SetActive(batteryBar == null);
						batteryBar?.SetActive(true);
						break;
				}
				foreach (ImageView iv in _gameEnergyUIPanel.GetComponentsInChildren<ImageView>())
				{
					iv.enabled = false;
				}
				foreach (Canvas ca in _gameEnergyUIPanel.GetComponentsInChildren<Canvas>())
				{
					ca.enabled = false;
				}
			}
		}
    }
}
