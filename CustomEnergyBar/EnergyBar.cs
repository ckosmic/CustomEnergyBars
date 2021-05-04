using CustomEnergyBar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar
{
	public class EnergyBar
	{
		public EnergyBarDescriptor descriptor;
		public GameObject energyBarPrefab { get; private set; }
		public string loadedFrom;

		private AssetBundle _bundle;

		public EnergyBar(string bundlePath) {
			if (!string.IsNullOrEmpty(bundlePath)) {
				_bundle = AssetBundle.LoadFromFile(bundlePath);
				ExtractBundle(_bundle);
			}
		}

		public EnergyBar(byte[] bundleData) {
			if (bundleData.Length > 0) {
				_bundle = AssetBundle.LoadFromMemory(bundleData);
				ExtractBundle(_bundle);
			}
		}

		private void ExtractBundle(AssetBundle assetBundle) {
			if (assetBundle != null) {
				energyBarPrefab = assetBundle.LoadAsset<GameObject>("_CustomEnergyBar");
				descriptor = energyBarPrefab.GetComponent<EnergyBarDescriptor>();
				if (descriptor.icon == null)
					descriptor.icon = ResourceUtilities.LoadSpriteFromResource($"CustomEnergyBar.Resources.icon.png");
				assetBundle.Unload(false);
			} else {
				return;
			}
		}

		public void Destroy() {
			if (_bundle != null) {
				_bundle.Unload(true);
			} else {
				UnityEngine.Object.Destroy(descriptor);
			}
		}
	}
}
