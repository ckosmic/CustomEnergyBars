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
		public GameObject energyBarPrefab;
		public AssetBundle bundle;

		public EnergyBar(string bundlePath) {
			if (!string.IsNullOrEmpty(bundlePath)) {
				bundle = AssetBundle.LoadFromFile(bundlePath);
				if (bundle != null) {
					energyBarPrefab = bundle.LoadAsset<GameObject>("_CustomEnergyBar");
					descriptor = energyBarPrefab.GetComponent<EnergyBarDescriptor>();
					if (descriptor.icon == null)
						descriptor.icon = ResourceUtilities.LoadSpriteFromResource($"CustomEnergyBar.Resources.icon.png");
					bundle.Unload(false);
				} else {
					return;
				}
			}
		}

		public void Destroy() {
			if (bundle != null) {
				bundle.Unload(true);
			} else {
				UnityEngine.Object.Destroy(descriptor);
			}
		}
	}
}
