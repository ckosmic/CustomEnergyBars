using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

namespace CustomEnergyBar
{
	[CustomEditor(typeof(SkewedTextMeshPro), true), CanEditMultipleObjects]
	public class SkewedTextMeshProEditor : TMPro.EditorUtilities.TMP_UiEditorPanel
	{
		SerializedProperty skewX;
		SerializedProperty skewY;

		protected override void OnEnable() {
			base.OnEnable();

			skewX = serializedObject.FindProperty("skewX");
			skewY = serializedObject.FindProperty("skewY");
		}

		public override void OnInspectorGUI() {
			SkewedTextMeshPro skewedTextMeshPro = (SkewedTextMeshPro)target;

			serializedObject.Update();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Skew");
			float prevWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 13;
			skewX.floatValue = EditorGUILayout.FloatField("X", skewX.floatValue);
			skewY.floatValue = EditorGUILayout.FloatField("Y", skewY.floatValue);
			EditorGUIUtility.labelWidth = prevWidth;
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();

			base.OnInspectorGUI();
		}
	}
}