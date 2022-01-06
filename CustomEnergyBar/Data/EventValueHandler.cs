using System;
using System.Collections;
using UnityEngine;

namespace CustomEnergyBar
{
	public abstract class EventValueHandler : MonoBehaviour
	{
		public string parameterName;
		public AnimationCurve interpolation = AnimationCurve.Constant(0, 0.01f, 1);

		protected Action<float> onFloatValueSet;
		protected Action<int> onIntValueSet;
		protected Action<bool> onBoolValueSet;
		protected Action onTriggerSet;
		protected Action<string> onStringValueSet;
		protected Action<Color> onColorValueSet;

		private float _oldFloatValue = 0f;
		private int _oldIntValue = 0;
		private Color _oldColorValue = Color.white;

		private Coroutine _interpolationCoroutine;

		public void SetFloat(float value) {
			if (gameObject.activeInHierarchy) {
				if (_interpolationCoroutine != null)
					StopCoroutine(_interpolationCoroutine);
				_interpolationCoroutine = StartCoroutine(InterpolateFloatValue(_oldFloatValue, value, onFloatValueSet));
			}
		}
		public void SetInteger(int value) {
			if (gameObject.activeInHierarchy) {
				if (_interpolationCoroutine != null)
					StopCoroutine(_interpolationCoroutine);
				_interpolationCoroutine = StartCoroutine(InterpolateIntValue(_oldIntValue, value, onIntValueSet));
			}
		}
		public void SetBool(bool value) {
			onBoolValueSet.Invoke(value);
		}
		public void SetTrigger() {
			onTriggerSet.Invoke();
		}
		public void SetString(string value) {
			onStringValueSet.Invoke(value);
		}
		public void SetColor(string value) {
			Color colorValue = Color.white;
			ColorUtility.TryParseHtmlString(value, out colorValue);
			if (gameObject.activeInHierarchy) {
				if (_interpolationCoroutine != null)
					StopCoroutine(_interpolationCoroutine);
				_interpolationCoroutine = StartCoroutine(InterpolateColorValue(_oldColorValue, colorValue, onColorValueSet));
			}
		}

		IEnumerator InterpolateFloatValue(float oldValue, float newValue, Action<float> floatValueSet) {
			float currentTime = Time.time;
			float curveTime = interpolation[interpolation.length - 1].time;
			float deltaTime = Time.time - currentTime;
			while (deltaTime < curveTime) {
				float value = Mathf.Lerp(oldValue, newValue, interpolation.Evaluate(deltaTime));
				floatValueSet.Invoke(value);
				yield return new WaitForEndOfFrame();
				deltaTime = Time.time - currentTime;
				_oldFloatValue = value;
			}
			_oldFloatValue = newValue;
			floatValueSet.Invoke(newValue);
		}

		IEnumerator InterpolateIntValue(int oldValue, int newValue, Action<int> intValueSet) {
			float currentTime = Time.time;
			float curveTime = interpolation[interpolation.length - 1].time;
			float deltaTime = Time.time - currentTime;
			while (deltaTime < curveTime) {
				int value = (int)Mathf.Lerp(oldValue, newValue, interpolation.Evaluate(deltaTime));
				intValueSet.Invoke(value);
				yield return new WaitForEndOfFrame();
				deltaTime = Time.time - currentTime;
				_oldFloatValue = value;
			}
			_oldFloatValue = newValue;
			intValueSet.Invoke(newValue);
		}

		IEnumerator InterpolateColorValue(Color oldValue, Color newValue, Action<Color> colorValueSet) {
			float currentTime = Time.time;
			float curveTime = interpolation[interpolation.length - 1].time;
			float deltaTime = Time.time - currentTime;
			while (deltaTime < curveTime) {
				Color value = Color.Lerp(oldValue, newValue, interpolation.Evaluate(deltaTime));
				colorValueSet.Invoke(value);
				yield return new WaitForEndOfFrame();
				deltaTime = Time.time - currentTime;
				_oldColorValue = value;
			}
			_oldColorValue = newValue;
			colorValueSet.Invoke(newValue);
		}
	}
}
