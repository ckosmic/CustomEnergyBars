using UnityEngine;
using UnityEngine.Events;

namespace CustomEnergyBar
{
	class EnergyEventManager : MonoBehaviour {

		private EventManager _eventManager;
		private GameObject _rootEnergyBarGO;
		private float _previousEnergy;
		private GameEnergyCounter _gameEnergyCounter;

		internal void Initialize(GameEnergyCounter gameEnergyCounter, EventManager eventManager, GameObject rootEnergyBarGO) {
			_gameEnergyCounter = gameEnergyCounter;
			_eventManager = eventManager;
			_rootEnergyBarGO = rootEnergyBarGO;

			_eventManager.OnInit.Invoke();
			_eventManager.DeserializeEvents();

			SubscribeToEvents();

			if (_gameEnergyCounter.energyType == GameplayModifiers.EnergyType.Bar)
				_previousEnergy = _gameEnergyCounter.energy;
			else
				_previousEnergy = 1.1f;

			OnEnergyChangedHandler(_gameEnergyCounter.energy);

			if (_gameEnergyCounter.energyType == GameplayModifiers.EnergyType.Bar && _eventManager.OnBatteryLivesDecreased.Length > 0) {
				int eventIndex = Mathf.CeilToInt(_gameEnergyCounter.energy * _eventManager.OnBatteryLivesDecreased.Length);
				for (int i = eventIndex; i < _eventManager.OnBatteryLivesDecreased.Length; i++) {
					_eventManager.OnBatteryLivesDecreased[i]?.Invoke();
				}
			}
		}

		private void OnDisable() {
			UnsubscribeFromEvents();
		}

		private void SubscribeToEvents() {
            _gameEnergyCounter.gameEnergyDidChangeEvent += OnEnergyChangedHandler;
			_gameEnergyCounter.gameEnergyDidReach0Event += OnEnergyReachedZeroHandler;
		}

        private void UnsubscribeFromEvents() {
			_gameEnergyCounter.gameEnergyDidChangeEvent -= OnEnergyChangedHandler;
			_gameEnergyCounter.gameEnergyDidReach0Event -= OnEnergyReachedZeroHandler;
		}

		private void OnEnergyReachedZeroHandler() {
			_eventManager.OnEnergyReachedZero?.Invoke();
			if(_eventManager.DeactivateOnEnergyReachedZero)
				_rootEnergyBarGO.SetActive(false);
		}

		private void OnEnergyChangedHandler(float energy) {
			_eventManager.OnEnergyChanged?.Invoke(energy);
			if (energy > _previousEnergy) {
				_eventManager.OnEnergyIncreased?.Invoke();
			} else if (energy < _previousEnergy) {
				_eventManager.OnEnergyDecreased?.Invoke();
			}

			UnityEvent[] onBatteryLivesIncreased = _eventManager.OnBatteryLivesIncreased;
			UnityEvent[] onBatteryLivesDecreased = _eventManager.OnBatteryLivesDecreased;

			if (_gameEnergyCounter.energyType == GameplayModifiers.EnergyType.Battery) {
				int eventIndex = _gameEnergyCounter.batteryEnergy;
				if (energy > _previousEnergy) {
					if (onBatteryLivesIncreased.Length > 0 && eventIndex < onBatteryLivesIncreased.Length)
						onBatteryLivesIncreased[eventIndex-1]?.Invoke();
				} else if (energy < _previousEnergy) {
					if (onBatteryLivesDecreased.Length > 0 && eventIndex < onBatteryLivesDecreased.Length)
						onBatteryLivesDecreased[eventIndex]?.Invoke();
				}
			} else {
				if (energy > _previousEnergy) {
					if (onBatteryLivesIncreased.Length > 0) {
						int eventIndex = Mathf.CeilToInt(energy * onBatteryLivesIncreased.Length);
						for (int i = 0; i < eventIndex; i++) {
							onBatteryLivesIncreased[i-1]?.Invoke();
						}
					}
				} else if (energy < _previousEnergy) {
					if (onBatteryLivesDecreased.Length > 0) {
						int eventIndex = Mathf.CeilToInt(energy * onBatteryLivesDecreased.Length);
						for (int i = eventIndex; i < onBatteryLivesDecreased.Length; i++) {
							onBatteryLivesDecreased[i]?.Invoke();
						}
					}
				}
			}
			_previousEnergy = energy;
		}
	}
}
