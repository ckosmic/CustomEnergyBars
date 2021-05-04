using CustomEnergyBar.Utils;
using CustomEnergyBar.API;
using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace CustomEnergyBar
{
	internal class EnergyBarManager : MonoBehaviour {
		public static EnergyBarManager Instance;

		public static void Load() {
			if (Instance != null) return;
			GameObject go = new GameObject("Energy Bar Manager");
			go.AddComponent<EnergyBarManager>();
		}

		private void Awake() {
			if (Instance != null) return;
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		private void OnEnable() {
			EnergyLoader.Load();
			AddBSEvents();
		}

		private void OnDisable() {
			RemoveBSEvents();
		}

		private void OnGameSceneLoaded() {
			if (Plugin.Settings.Selected != "defaultEnergyBar") {
				EnergyBar energyBar = (CEBAPI.overrideBar != null) ? CEBAPI.overrideBar : EnergyLoader.GetEnergyBarByBundleId(Plugin.Settings.Selected);
				InstantiateEnergyBar(energyBar);
			}
		}

		public void AddManagers(GameObject energyGo) {
			if (energyGo.GetComponentInChildren<EventManager>(true) != null) {
				foreach (EventManager manager in energyGo.GetComponentsInChildren<EventManager>(true)) {
					EnergyEventManager eem = manager.gameObject.AddComponent<EnergyEventManager>();
					eem.eventManager = manager;
					eem.rootEnergyBar = energyGo;
				}
			}
			if (Plugin.Settings.AllowSFX == false && energyGo.GetComponentInChildren<AudioSource>(true) != null) {
				foreach (AudioSource audio in energyGo.GetComponentsInChildren<AudioSource>()) {
					Destroy(audio);
				}
			}
		}

		private void AddBSEvents() {
			RemoveBSEvents();
			BS_Utils.Utilities.BSEvents.gameSceneLoaded += OnGameSceneLoaded;
		}

		private void RemoveBSEvents() {
			BS_Utils.Utilities.BSEvents.gameSceneLoaded -= OnGameSceneLoaded;
		}

		public void InstantiateEnergyBar(EnergyBar energyBar) {
			StartCoroutine(IEInstantiateEnergyBar(energyBar));
		}

		IEnumerator IEInstantiateEnergyBar(EnergyBar energyBar) {
			yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().Any());
			GameEnergyUIPanel originalEnergyUI = Resources.FindObjectsOfTypeAll<GameEnergyUIPanel>().FirstOrDefault();
			yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<GameEnergyCounter>().Any());
			GameEnergyCounter energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().FirstOrDefault();
			if (originalEnergyUI.gameObject.activeInHierarchy) {
				GameObject prefab = energyBar.energyBarPrefab;
				GameObject go = Instantiate(prefab, originalEnergyUI.transform.position, originalEnergyUI.transform.rotation);
				AddManagers(go);
				EnergyBarDescriptor descriptor = go.GetComponent<EnergyBarDescriptor>();
				switch (energyCounter.energyType) {
					case GameplayModifiers.EnergyType.Bar:
						descriptor.standardBar.gameObject.SetActive(true);
						break;
					case GameplayModifiers.EnergyType.Battery:
						int lives = energyCounter.batteryLives;
						GameObject batteryBar = descriptor.GetBatteryBar(lives);
						descriptor.standardBar.gameObject.SetActive(batteryBar == null);
						batteryBar?.SetActive(true);
						break;
				}
				foreach (ImageView iv in originalEnergyUI.GetComponentsInChildren<ImageView>()) {
					iv.enabled = false;
				}
			}
		}
	}
}
