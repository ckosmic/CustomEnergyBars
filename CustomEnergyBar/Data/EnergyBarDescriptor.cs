using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar
{
	public class EnergyBarDescriptor : MonoBehaviour
	{
		public string name = "MyCustomEnergyBar";
		public string author = "unknown";
		public string description = "A custom energy bar!";
		public Sprite icon;

		[HideInInspector]
		public string bundleId = "unknown.MyCustomEnergyBar";
		[HideInInspector]
		public GameObject standardBar;
		[HideInInspector]
		public List<GameObject> batteryBars;

		public GameObject GetBatteryBar(int batteryLives) {
			for (int i = 0; i < batteryBars.Count; i++) {
				if (batteryBars[i].name == batteryLives.ToString() + "LifeBar") {
					return batteryBars[i];
				}
			}
			return null;
		}
	}
}
