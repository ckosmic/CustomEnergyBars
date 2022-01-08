using CustomEnergyBar.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using Zenject;
using UnityEngine;

namespace CustomEnergyBar
{
	public class EnergyLoader
	{
		private const string _customFolder = "CustomEnergyBars";

		public bool IsLoaded { get; private set; } = false;
		public int SelectedEnergyBar { get; internal set; } = 0;
		public List<EnergyBar> CustomEnergyBars { get; private set; } = new List<EnergyBar>();
		public List<EnergyBar> APIEnergyBars { get; private set; } = new List<EnergyBar>();
		public List<string> AssetBundlePaths { get; private set; } = new List<string>();
		public GameObject PrefabPool = null;

		internal void Load() {
			if (!IsLoaded) {
				string customPath = Path.Combine(Environment.CurrentDirectory, _customFolder);

				if (!Directory.Exists(customPath)) {
					Directory.CreateDirectory(_customFolder);
				}

				PrefabPool = new GameObject("CEB Prefab Pool");
				PrefabPool.SetActive(false);
				UnityEngine.Object.DontDestroyOnLoad(PrefabPool);

				string[] bundlePaths = Directory.GetFiles(customPath, "*.energy");

				LoadCustomEnergyBars(bundlePaths);
				Plugin.Log.Info("Loaded " + bundlePaths.Length + " custom energy bars.");

				if (Plugin.Settings.Selected != "defaultEnergyBar") {
					SelectedEnergyBar = CustomEnergyBars.FindIndex(ceb => ceb.descriptor.bundleId == Plugin.Settings.Selected);
				}

				IsLoaded = true;
			}
		}

		internal void Reload() {
			Clear();
			Load();
		}

		internal void Clear() {
			for (int i = 0; i < CustomEnergyBars.Count; i++) {
				CustomEnergyBars[i].Destroy();
				CustomEnergyBars[i] = null;
			}

			IsLoaded = false;
			SelectedEnergyBar = 0;
			CustomEnergyBars = new List<EnergyBar>();
			AssetBundlePaths = new List<string>();
			UnityEngine.Object.Destroy(PrefabPool);
		}

		public void LoadCustomEnergyBars(string[] assetBundlePaths) {
			EnergyBar defaultEnergyBar = new EnergyBar("", PrefabPool);
			defaultEnergyBar.descriptor = new EnergyBarDescriptor();
			defaultEnergyBar.descriptor.name = "Default";
			defaultEnergyBar.descriptor.author = "Beat Saber";
			defaultEnergyBar.descriptor.description = "The default energy bar.";
			defaultEnergyBar.descriptor.bundleId = "defaultEnergyBar";
			defaultEnergyBar.descriptor.icon = ResourceUtilities.LoadSpriteFromResource("CustomEnergyBar.Resources.icon.png");

			AssetBundlePaths.Add("");
			CustomEnergyBars.Add(defaultEnergyBar);

			foreach (string path in assetBundlePaths) {
				EnergyBar energyBar = new EnergyBar(path, PrefabPool);
				if (energyBar != null) {
					AssetBundlePaths.Add(path);
					CustomEnergyBars.Add(energyBar);
				}
			}
		}

		public EnergyBar GetEnergyBarByBundleId(string bundleId) {
			return CustomEnergyBars.Find(ceb => ceb.descriptor.bundleId == bundleId);
		}

		public EnergyBar GetAPIEnergyBarByBundleId(string bundleId) {
			return APIEnergyBars.Find(ceb => ceb.descriptor.bundleId == bundleId);
		}

		public void AddAPIEnergyBar(EnergyBar energyBar) {
			APIEnergyBars.Add(energyBar);
		}
	}
}
