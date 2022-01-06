using System.Collections.Generic;
using UnityEngine;

namespace CustomEnergyBar
{
	[AddComponentMenu("Custom Energy Bars/Energy Bar Descriptor")]
	public class EnergyBarDescriptor : MonoBehaviour
	{
		public string name = "MyCustomEnergyBar";
		public string author = "unknown";
		[TextArea(3, 5)]
		public string description = "A custom energy bar!";
		public Sprite icon;

		[HideInInspector]
		public string bundleId = "unknown.MyCustomEnergyBar";
		[HideInInspector]
		public GameObject standardBar;
		[HideInInspector]
		public List<GameObject> batteryBars;

		public GameObject GetBatteryBar(int batteryLives) {
			return batteryBars.Find(bb => bb.name == batteryLives.ToString() + "LifeBar");
		}
	}
}
