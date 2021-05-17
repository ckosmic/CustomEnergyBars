using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEnergyBar;

public class EventTester : MonoBehaviour {

	public EventManager eventManager;
	public float onEnergyChangedValue = 1.0f;
	public int batteryLives = 4;

	public bool autoOnEnergyChanged = false;

	public int _maxBatteryLives = 4;

	private void Start() {
		_maxBatteryLives = batteryLives;
		InvokeOnInit();
		InvokeOnEnergyChanged(onEnergyChangedValue);
	}

	public void InvokeOnInit() {
		eventManager.OnInit.Invoke();
	}

	public void InvokeOnEnergyReachedZero() {
		eventManager.OnEnergyReachedZero.Invoke();
	}

	public void InvokeOnEnergyIncreased() {
		eventManager.OnEnergyIncreased.Invoke();
	}

	public void InvokeOnEnergyDecreased() {
		eventManager.OnEnergyDecreased.Invoke();
	}

	public void InvokeOnEnergyChanged(float value) {
		eventManager.OnEnergyChanged.Invoke(value);
	}

	public void InvokeOnBatteryLivesIncreased(int index) {
		eventManager.OnBatteryLivesIncreased[index-1].Invoke();
	}

	public void InvokeOnBatteryLivesDecreased(int index) {
		eventManager.OnBatteryLivesDecreased[index].Invoke();
	}
}
