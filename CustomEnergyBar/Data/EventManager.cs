using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
#if UNITY
using UnityEditor;
#endif
using Object = UnityEngine.Object;
using CustomEnergyBar.Utils;
using System.Collections.Generic;
#if PLUGIN
using IPA.Utilities;
#endif

namespace CustomEnergyBar
{
	[AddComponentMenu("Custom Energy Bars/Event Manager")]
	public class EventManager : MonoBehaviour {
		public UnityEvent OnInit;
		public UnityEvent OnEnergyReachedZero;
		public bool DeactivateOnEnergyReachedZero = true;
		public UnityEvent OnEnergyIncreased;
		public UnityEvent OnEnergyDecreased;
		public UnityEventFloat OnEnergyChanged;

		public UnityEvent[] OnBatteryLivesIncreased;
		public UnityEvent[] OnBatteryLivesDecreased;

		#region Serialization
		// All this just to get a UnityEvent<T> superclass to serialize in asset bundles... took like 3 days to figure out.

		[SerializeField, HideInInspector]
		private string json_onEnergyChanged;
		[SerializeField, HideInInspector]
		private Object[] targets_onEnergyChanged;

		public void SerializeEvents() {
#if UNITY
			targets_onEnergyChanged = GetTargets(this, "OnEnergyChanged");
#endif
			json_onEnergyChanged = JsonUtility.ToJson(OnEnergyChanged);
		}

		public void DeserializeEvents() {
			if (!string.IsNullOrEmpty(json_onEnergyChanged)) {
				OnEnergyChanged = JsonUtility.FromJson<UnityEventFloat>(json_onEnergyChanged);
				AssignTargets(OnEnergyChanged, targets_onEnergyChanged);
			}
		}

		public static void AssignTargets(UnityEventBase unityEvent, Object[] objects) {
#if PLUGIN
			BindingFlags bindings = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
			Type PersistentCall = typeof(UnityEventBase).Assembly.GetType("UnityEngine.Events.PersistentCall");
			var m_PersistentCalls = typeof(UnityEventBase).GetField("m_PersistentCalls", bindings);
			var m_Calls = m_PersistentCalls.FieldType.GetField("m_Calls", bindings);
			var _items = m_Calls.FieldType.GetField("_items", bindings);

			object persistentCalls = m_PersistentCalls.GetValue(unityEvent);
			object calls = m_Calls.GetValue(persistentCalls);
			object[] items = (object[])_items.GetValue(calls);

			var m_Target = PersistentCall.GetField("m_Target", bindings);
			for (int i = 0; i < items.Length; i++) {
				m_Target.SetValue(items[i], objects[i]);
			}
#endif
		}

#if UNITY
		public static Object[] GetTargets(Object obj, string eventName) {
			Object[] targets;
			SerializedObject so = new SerializedObject(obj);
			SerializedProperty persistentCalls = so.FindProperty(eventName).FindPropertyRelative("m_PersistentCalls.m_Calls");
			targets = new Object[persistentCalls.arraySize];
			for (int i = 0; i < persistentCalls.arraySize; i++) {
				targets[i] = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Target").objectReferenceValue;
			}
			return targets;
		}
#endif
		#endregion
	}

	[Serializable]
	public class UnityEventFloat : UnityEvent<float> { }
}
