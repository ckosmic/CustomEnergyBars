using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomEnergyBar.Data
{
	[AddComponentMenu("Custom Energy Bars/Animator Event Handler")]
	public class AnimatorEventHandler : MonoBehaviour
	{
		public Animator animator;
		public string parameterName;

		public void SetFloat(float value) {
			animator.SetFloat(parameterName, value);
		}
		public void SetInteger(int value) {
			animator.SetInteger(parameterName, value);
		}
		public void SetBool(bool value) {
			animator.SetBool(parameterName, value);
		}
		public void SetTrigger() {
			animator.SetTrigger(parameterName);
		}
	}
}
