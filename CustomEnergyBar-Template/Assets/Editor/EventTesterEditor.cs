using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEditor.Events;
using UnityEngine.Events;

namespace CustomEnergyBar
{
	[CustomEditor(typeof(EventTester))]
	public class EventTesterEditor : Editor {
		private EventTester eventTester;
		private float prevEnergy;

		SerializedProperty eventManager;
		SerializedProperty onEnergyChangedValue;
		SerializedProperty batteryLives;
		SerializedProperty autoOnEnergyChanged;

		private void OnEnable() {
			eventTester = (EventTester)target;

			eventManager = serializedObject.FindProperty("eventManager");
			onEnergyChangedValue = serializedObject.FindProperty("onEnergyChangedValue");
			batteryLives = serializedObject.FindProperty("batteryLives");
			autoOnEnergyChanged = serializedObject.FindProperty("autoOnEnergyChanged");

			prevEnergy = onEnergyChangedValue.floatValue;
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.LabelField("Note: This script will only work in play mode.");
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(eventManager);

			if (GUILayout.Button("Invoke OnInit()")) {
				eventTester.InvokeOnInit();
			}
			if (GUILayout.Button("Invoke OnEnergyReachedZero()")) {
				eventTester.InvokeOnEnergyReachedZero();
			}
			if (GUILayout.Button("Invoke OnEnergyIncreased()")) {
				eventTester.InvokeOnEnergyIncreased();
			}
			if (GUILayout.Button("Invoke OnEnergyDecreased()")) {
				eventTester.InvokeOnEnergyDecreased();
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			onEnergyChangedValue.floatValue = EditorGUILayout.Slider(onEnergyChangedValue.floatValue, 0.0f, 1.0f);
			if (GUILayout.Button("Invoke OnEnergyChanged(Single)")) {
				eventTester.InvokeOnEnergyChanged(onEnergyChangedValue.floatValue);
			}
			EditorGUILayout.EndHorizontal();
			autoOnEnergyChanged.boolValue = EditorGUILayout.Toggle("Auto-Invoke", autoOnEnergyChanged.boolValue);
			if (EditorGUI.EndChangeCheck() && autoOnEnergyChanged.boolValue) {
				eventTester.InvokeOnEnergyChanged(onEnergyChangedValue.floatValue);
				if (onEnergyChangedValue.floatValue > prevEnergy) {
					eventTester.InvokeOnEnergyIncreased();
				} else if (onEnergyChangedValue.floatValue < prevEnergy) {
					eventTester.InvokeOnEnergyDecreased();
				}
				prevEnergy = onEnergyChangedValue.floatValue;
			}

			if (GUILayout.Button("Invoke OnBatteryLivesIncreased()")) {
				batteryLives.intValue++;
				eventTester.InvokeOnBatteryLivesIncreased(batteryLives.intValue);
				onEnergyChangedValue.floatValue = (float)batteryLives.intValue / (float)eventTester._maxBatteryLives;
				eventTester.InvokeOnEnergyChanged(onEnergyChangedValue.floatValue);
			}
			if (GUILayout.Button("Invoke OnBatteryLivesDecreased()")) {
				batteryLives.intValue--;
				eventTester.InvokeOnBatteryLivesDecreased(batteryLives.intValue);
				onEnergyChangedValue.floatValue = (float)batteryLives.intValue / (float)eventTester._maxBatteryLives;
				eventTester.InvokeOnEnergyChanged(onEnergyChangedValue.floatValue);
			}
			if (Application.isPlaying) {
				EditorGUILayout.LabelField("Battery Lives: " + batteryLives.intValue);
			} else {
				batteryLives.intValue = EditorGUILayout.IntField("Battery Lives", batteryLives.intValue);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}