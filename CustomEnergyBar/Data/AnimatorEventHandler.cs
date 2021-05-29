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
	public class AnimatorEventHandler : MonoBehaviour
	{
		public Animator animator;
		public string parameterName;
		public AnimationCurve interpolation = AnimationCurve.Constant(0, 0.01f, 1);

		private float _oldValue = 0.5f;
		private Coroutine _interpolationCoroutine;

		public void SetFloat(float value) {
			if(_interpolationCoroutine != null)
				StopCoroutine(_interpolationCoroutine);
			_interpolationCoroutine = StartCoroutine(InterpolateValue(_oldValue, value));
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

		IEnumerator InterpolateValue(float oldValue, float newValue) {
			float currentTime = Time.time;
			float curveTime = interpolation[interpolation.length - 1].time;
			float deltaTime = Time.time - currentTime;
			while (deltaTime < curveTime) {
				float value = Mathf.Lerp(oldValue, newValue, interpolation.Evaluate(deltaTime));
				animator.SetFloat(parameterName, value);
				yield return new WaitForEndOfFrame();
				deltaTime = Time.time - currentTime;
				_oldValue = value;
			}
			_oldValue = newValue;
			animator.SetFloat(parameterName, newValue);
		}
	}
}
