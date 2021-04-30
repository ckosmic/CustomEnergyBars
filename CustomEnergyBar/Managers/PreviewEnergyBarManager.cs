using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEnergyBar
{
	internal class PreviewEnergyBarManager : MonoBehaviour
	{
		public static PreviewEnergyBarManager Instance;

		private bool _isSimulating = false;
		private float _simulatedEnergy = 0.5f;
		private float _previousEnergy = 0.5f;
		private Coroutine _simCoroutine;

		public static void Load() {
			if (Instance != null) return;
			GameObject go = new GameObject("Preview Energy Bar Manager");
			go.AddComponent<PreviewEnergyBarManager>();
		}

		private void Awake() {
			if (Instance != null) return;
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		public void StartSimulation(GameObject energyGo) {
			if (energyGo.GetComponentInChildren<EventManager>(true) != null) {
				foreach (EventManager em in energyGo.GetComponentsInChildren<EventManager>(true)) {
					em.DeserializeEvents();
				}
			}
			if (Plugin.Settings.AllowSFX == false && energyGo.GetComponentInChildren<AudioSource>(true) != null) {
				foreach (AudioSource audio in energyGo.GetComponentsInChildren<AudioSource>()) {
					Destroy(audio);
				}
			}

			_simCoroutine = StartCoroutine(IEPreviewEnergySimulation(energyGo));
		}

		public void StopSimulation() {
			_isSimulating = false;
			if (_simCoroutine != null) {
				StopCoroutine(_simCoroutine);
			}
			_simulatedEnergy = 0.5f;
			_previousEnergy = 0.5f;
		}

		private void InvokeEvent(EventManager[] eventManagers, string eventName) {
			foreach (EventManager em in eventManagers) {
				((UnityEvent)typeof(EventManager).GetField(eventName).GetValue(em)).Invoke();
			}
		}

		private void InvokeAll(EventManager[] eventManagers, float energy) {
			foreach (EventManager em in eventManagers) {
				em.OnEnergyChanged?.Invoke(energy);
				if (energy > _previousEnergy) {
					em.OnEnergyIncreased?.Invoke();
				} else if (energy < _previousEnergy) {
					em.OnEnergyDecreased?.Invoke();
				}
				_previousEnergy = energy;
			}
		}

		IEnumerator IEPreviewEnergySimulation(GameObject energyGo) {
			_isSimulating = true;

			EventManager[] eventManagers = energyGo.GetComponentsInChildren<EventManager>();

			InvokeEvent(eventManagers, "OnInit");
			while (_isSimulating) {
				_simulatedEnergy = 0.5f;
				_previousEnergy = 0.5f;
				InvokeAll(eventManagers, _simulatedEnergy);

				yield return new WaitForSecondsRealtime(0.5f);
				for (int i = 0; i < 4; i++) {
					_simulatedEnergy -= 0.1f;
					InvokeAll(eventManagers, _simulatedEnergy);
					yield return new WaitForSecondsRealtime(0.5f);
				}
				for (int i = 0; i < 16; i++) {
					_simulatedEnergy += 0.05f;
					InvokeAll(eventManagers, _simulatedEnergy);
					yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0.1f, 0.25f));
				}
			}
		}
	}
}
