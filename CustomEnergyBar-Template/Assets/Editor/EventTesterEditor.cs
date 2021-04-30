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

		SerializedProperty eventManager;
		SerializedProperty onEnergyChangedValue;

		private void OnEnable() {
			eventTester = (EventTester)target;

			eventManager = serializedObject.FindProperty("eventManager");
			onEnergyChangedValue = serializedObject.FindProperty("onEnergyChangedValue");
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
			EditorGUILayout.BeginHorizontal();
			onEnergyChangedValue.floatValue = EditorGUILayout.FloatField(onEnergyChangedValue.floatValue);
			if (GUILayout.Button("Invoke OnEnergyChanged(Single)")) {
				eventTester.InvokeOnEnergyChanged(onEnergyChangedValue.floatValue);
			}
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}
	}
}