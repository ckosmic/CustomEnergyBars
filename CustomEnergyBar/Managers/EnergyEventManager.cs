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
			Plugin.Log.Info("Subscribing to events...");

			BSEvents.energyDidChange += OnEnergyChangedHandler;
			BSEvents.energyReachedZero += OnEnergyReachedZeroHandler;

			Plugin.Log.Info("OnEnergyChanged Event Count: " + eventManager.OnEnergyChanged.GetPersistentEventCount());
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
					if (eventManager.OnBatteryLivesIncreased.Length > 0)
						eventManager.OnBatteryLivesIncreased[eventIndex]?.Invoke();
				} else if (energy < _previousEnergy) {
					if (eventManager.OnBatteryLivesDecreased.Length > 0)
						eventManager.OnBatteryLivesDecreased[eventIndex]?.Invoke();
				}
			}
			_previousEnergy = energy;
		}

		private void OnDisable() {
			UnsubscribeFromEvents();
		}

		IEnumerator IEEventManagerInitialization() {
			Plugin.Log.Info("Initializing event manager...");
			yield return new WaitUntil(() => eventManager != null );
			eventManager.OnInit.Invoke();
			eventManager.DeserializeEvents();
			SubscribeToEvents();
			_energyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().FirstOrDefault();
			eventManager.OnEnergyChanged?.Invoke(_energyCounter.energy);
			_previousEnergy = _energyCounter.energy;
		}
	}
}
