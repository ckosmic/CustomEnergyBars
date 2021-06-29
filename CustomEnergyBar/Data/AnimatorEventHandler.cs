using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar.Data
{
	[AddComponentMenu("Custom Energy Bars/Animator Event Handler")]
	public class AnimatorEventHandler : EventValueHandler
	{
		public Animator animator;

		private void Awake() {
			onFloatValueSet += OnFloatValueSet;
			onIntValueSet += OnIntegerValueSet;
			onBoolValueSet += OnBoolValueSet;
			onTriggerSet += OnTriggerSet;
		}

		private void OnDestroy() {
			onFloatValueSet -= OnFloatValueSet;
			onIntValueSet -= OnIntegerValueSet;
			onBoolValueSet -= OnBoolValueSet;
			onTriggerSet -= OnTriggerSet;
		}

		private void OnFloatValueSet(float value) {
			animator.SetFloat(parameterName, value);
		}
		private void OnIntegerValueSet(int value) {
			animator.SetInteger(parameterName, value);
		}
		private void OnBoolValueSet(bool value) {
			animator.SetBool(parameterName, value);
		}
		private void OnTriggerSet() {
			animator.SetTrigger(parameterName);
		}
	}
}
