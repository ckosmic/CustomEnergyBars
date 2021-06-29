using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar
{
	[AddComponentMenu("Custom Energy Bars/Material Event Handler")]
	public class MaterialEventHandler : EventValueHandler
	{
		public Material material;

		private void Awake() {
			onFloatValueSet += OnFloatValueSet;
			onIntValueSet += OnIntegerValueSet;
			onColorValueSet += OnColorValueSet;
		}

		private void OnDestroy() {
			onFloatValueSet -= OnFloatValueSet;
			onIntValueSet -= OnIntegerValueSet;
			onColorValueSet -= OnColorValueSet;
		}

		private void OnFloatValueSet(float value) {
			material.SetFloat(parameterName, value);
		}
		private void OnIntegerValueSet(int value) {
			material.SetInt(parameterName, value);
		}
		private void OnColorValueSet(Color color) {
			material.SetColor(parameterName, color);
		}
	}
}
