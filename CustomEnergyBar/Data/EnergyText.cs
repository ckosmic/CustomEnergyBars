using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace CustomEnergyBar
{
	public class EnergyText : MonoBehaviour
	{
		public enum DisplayTypes { 
			Decimal,
			Percent
		}

		public DisplayTypes displayType;
		public string formattedString = "Energy: {0}";

		private TextMeshProUGUI _tmpro;

		public void UpdateEnergyText(float energy) {
			if(_tmpro == null) 
				_tmpro = GetComponent<TextMeshProUGUI>();

			string energyString = energy.ToString();
			switch (displayType) {
				case DisplayTypes.Decimal:
					energyString = ((Mathf.Round(energy * 100)) / 100.0f).ToString();
					break;
				case DisplayTypes.Percent:
					energyString = Mathf.Round(energy * 100).ToString() + "%";
					break;
			}
			_tmpro.text = string.Format(formattedString, energyString);
		}
	}
}
