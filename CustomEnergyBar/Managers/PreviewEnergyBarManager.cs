using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEnergyBar
{
	internal class PreviewEnergyBarManager
	{
		private float _simulatedEnergy = 0.5f;
		private float _previousEnergy = 0.5f;
		private BackgroundWorker _simWorker;

		public void StartSimulation(GameObject energyGo) {
			if (energyGo.GetComponentInChildren<EventManager>(true) != null) {
				foreach (EventManager em in energyGo.GetComponentsInChildren<EventManager>(true)) {
					em.DeserializeEvents();
				}
			}
			if (Plugin.Settings.AllowSFX == false && energyGo.GetComponentInChildren<AudioSource>(true) != null) {
				foreach (AudioSource audio in energyGo.GetComponentsInChildren<AudioSource>()) {
					UnityEngine.Object.Destroy(audio);
				}
			}

			StopSimulation();
			_simWorker = new BackgroundWorker();
			_simWorker.DoWork += PreviewEnergySimulation;
			_simWorker.WorkerSupportsCancellation = true;
			_simWorker.RunWorkerAsync(argument: energyGo);
		}

		public void StopSimulation() {
			if (_simWorker != null) {
				_simWorker.CancelAsync();
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
			foreach (EventManager eventManager in eventManagers) {
				eventManager.OnEnergyChanged?.Invoke(energy);
				if (energy > _previousEnergy) {
					eventManager.OnEnergyIncreased?.Invoke();
				} else if (energy < _previousEnergy) {
					eventManager.OnEnergyDecreased?.Invoke();
				}
				if (energy > _previousEnergy) {
					if (eventManager.OnBatteryLivesIncreased.Length > 0) {
						int eventIndex = Mathf.CeilToInt(energy * eventManager.OnBatteryLivesIncreased.Length);
						for (int i = 0; i < eventIndex; i++) {
							eventManager.OnBatteryLivesIncreased[i]?.Invoke();
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
				_previousEnergy = energy;
			}
		}

		private void PreviewEnergySimulation(object sender, DoWorkEventArgs args) { 
			GameObject energyGo = args.Argument as GameObject;
			EventManager[] eventManagers = energyGo.GetComponentsInChildren<EventManager>();
			BackgroundWorker worker = sender as BackgroundWorker;

			InvokeEvent(eventManagers, "OnInit");

			while (true) {
				_simulatedEnergy = 0.5f;
				_previousEnergy = 0.5f;
				InvokeAll(eventManagers, _simulatedEnergy);

				if (worker.CancellationPending) {
					args.Cancel = true;
					break;
				} else {
					Thread.Sleep(500);
				}
				
				for (int i = 0; i < 4; i++) {
					_simulatedEnergy -= 0.1f;
					InvokeAll(eventManagers, _simulatedEnergy);
					if (worker.CancellationPending) {
						break;
					} else {
						Thread.Sleep(500);
					}
				}
				if (worker.CancellationPending) {
					args.Cancel = true;
					break;
				}
				for (int i = 0; i < 16; i++) {
					_simulatedEnergy += 0.05f;
					InvokeAll(eventManagers, _simulatedEnergy);
					if (worker.CancellationPending) {
						break;
					} else {
						Thread.Sleep(150);
					}
				}
				if (worker.CancellationPending) {
					args.Cancel = true;
					break;
				}
			}
		}
	}
}
