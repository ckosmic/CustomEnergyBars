using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BS_Utils.Utilities;
using System.Collections;

namespace CustomEnergyBar
{
	class EnergyEventManager : MonoBehaviour {
		public EventManager eventManager;
		public GameObject rootEnergyBar;

		private float _previousEnergy;
		private GameEnergyCounter _energyCounter;

		private void OnEnable() {
			StartCoroutine(IEEventManagerInitialization());
		}

		private void SubscribeToEvents() {
			BSEvents.energyDidChange += OnEnergyChangedHandler;
			BSEvents.energyReachedZero += OnEnergyReachedZeroHandler;
		}

		private void UnsubscribeFromEvents() {
			BSEvents.energyDidChange -= OnEnergyChangedHandler;
			BSEvents.energyReachedZero -= OnEnergyReachedZeroHandler;
		}

		private void OnEnergyReachedZeroHandler() {
			eventManager.OnEnergyReachedZero?.Invoke();
			if(eventManager.DeactivateOnEnergyReachedZero)
				rootEnergyBar.SetActive(false);
		}

		private void OnEnergyChangedHandler(float energy) {
			eventManager.OnEnergyChanged?.Invoke(energy);
			if (energy > _previousEnergy) {
				eventManager.OnEnergyIncreased?.Invoke();
			} else if (energy < _previousEnergy) {
				eventManager.OnEnergyDecreased?.Invoke();
			}
			if (_energyCounter.energyType == GameplayModifiers.EnergyType.Battery) {
				int eventIndex = _energyCounter.batteryEnergy;
				if (energy > _previousEnergy) {
					if (eventManager.OnBatteryLivesIncreased.Length > 0 && eventIndex < eventManager.OnBatteryLivesIncreased.Length)
						eventManager.OnBatteryLivesIncreased[eventIndex-1]?.Invoke();
				} else if (energy < _previousEnergy) {
					if (eventManager.OnBatteryLivesDecreased.Length > 0 && eventIndex < eventManager.OnBatteryLivesDecreased.Length)
						eventManager.OnBatteryLivesDecreased[eventIndex]?.Invoke();
				}
			} else {
				if (energy > _previousEnergy) {
					if (eventManager.OnBatteryLivesIncreased.Length > 0) {
						int eventIndex = Mathf.CeilToInt(energy * eventManager.OnBatteryLivesIncreased.Length);
						for (int i = 0; i < eventIndex; i++) {
							eventManager.OnBatteryLivesIncreased[i-1]?.Invoke();
						}
					}
				} else if (energy < _previousEnergy) {
					if (eventManager.OnBatteryLivesDecreased.Length > 0) {
						int eventIndex = Mathf.CeilToInt(energy * eventManager.OnBatteryLivesDecreased.Length);
						for (int i = eventIndex; i < eventManager.OnBatteryLivesDecreased.Length; i++) {
							eventManager.OnBatteryLivesDecreased[i]?.Invoke();
						}
					}
				}
			}
			_previousEnergy = energy;
		}

		private void OnDisable() {
			UnsubscribeFromEvents();
		}

		IEnumerator IEEventManagerInitialization() {
			yield return new WaitUntil(() => eventManager != null);
			eventManager.OnInit.Invoke();
			eventManager.DeserializeEvents();
			SubscribeToEvents();
			_energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().FirstOrDefault();
			if (_energyCounter.energyType == GameplayModifiers.EnergyType.Bar)
				_previousEnergy = _energyCounter.energy;
			else
				_previousEnergy = 1.1f;
			OnEnergyChangedHandler(_energyCounter.energy);
			
			if (_energyCounter.energyType == GameplayModifiers.EnergyType.Bar) {
				if (eventManager.OnBatteryLivesDecreased.Length > 0) {
					int eventIndex = Mathf.CeilToInt(_energyCounter.energy * eventManager.OnBatteryLivesDecreased.Length);
					for (int i = eventIndex; i < eventManager.OnBatteryLivesDecreased.Length; i++) {
						eventManager.OnBatteryLivesDecreased[i]?.Invoke();
					}
				}
			}
		}
	}
}
