using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEnergyBar;

public class EventTester : MonoBehaviour {

	public EventManager eventManager;
	public float onEnergyChangedValue = 1.0f;

	public bool autoOnEnergyChanged = false;

	private void Start() {
		InvokeOnInit();
		InvokeOnEnergyChanged(0.5f);
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
}
