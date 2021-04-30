using CustomEnergyBar.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar
{
	public class EnergyLoader
	{
		private const string _customFolder = "CustomEnergyBars";

		public static bool IsLoaded { get; private set; } = false;
		public static int SelectedEnergyBar { get; internal set; } = 0;
		public static List<EnergyBar> CustomEnergyBars { get; private set; } = new List<EnergyBar>();
		public static List<string> AssetBundlePaths { get; private set; } = new List<string>();

		internal static void Load() {
			if (!IsLoaded) {
				string customPath = Path.Combine(Environment.CurrentDirectory, _customFolder);

				if (!Directory.Exists(customPath)) {
					Directory.CreateDirectory(_customFolder);
				}

				string[] bundlePaths = Directory.GetFiles(customPath, "*.energy");

				LoadCustomEnergyBars(bundlePaths);
				Plugin.Log.Info("Loaded " + bundlePaths.Length + " custom energy bars.");

				if (Plugin.Settings.Selected != "defaultEnergyBar") {
					for (int i = 0; i < CustomEnergyBars.Count; i++) {
						if (CustomEnergyBars[i].descriptor.bundleId == Plugin.Settings.Selected) {
							SelectedEnergyBar = i;
							break;
						}
					}
				}

				IsLoaded = true;
			}
		}

		internal static void Reload() {
			Clear();
			Load();
		}

		internal static void Clear() {
			for (int i = 0; i < CustomEnergyBars.Count; i++) {
				CustomEnergyBars[i].Destroy();
				CustomEnergyBars[i] = null;
			}

			IsLoaded = false;
			SelectedEnergyBar = 0;
			CustomEnergyBars = new List<EnergyBar>();
			AssetBundlePaths = new List<string>();
		}

		public static void LoadCustomEnergyBars(string[] assetBundlePaths) {
			EnergyBar defaultEnergyBar = new EnergyBar("");
			defaultEnergyBar.descriptor = new EnergyBarDescriptor();
			defaultEnergyBar.descriptor.name = "Default";
			defaultEnergyBar.descriptor.author = "Beat Saber";
			defaultEnergyBar.descriptor.bundleId = "defaultEnergyBar";
			defaultEnergyBar.descriptor.icon = ResourceUtilities.LoadSpriteFromResource("CustomEnergyBar.Resources.icon.png");

			AssetBundlePaths.Add("");
			CustomEnergyBars.Add(defaultEnergyBar);

			foreach (string path in assetBundlePaths) {
				EnergyBar energyBar = new EnergyBar(path);
				if (energyBar != null) {
					AssetBundlePaths.Add(path);
					CustomEnergyBars.Add(energyBar);
				}
			}
		}

		public static EnergyBar GetEnergyBarByBundleId(string bundleId) {
			foreach (EnergyBar eb in CustomEnergyBars) {
				if (eb.descriptor.bundleId == bundleId)
					return eb;
			}
			return null;
		}
	}
}
