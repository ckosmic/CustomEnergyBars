using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEditor.Events;
using UnityEngine.Events;
using System;

namespace CustomEnergyBar
{
	[CustomEditor(typeof(EnergyBarDescriptor))]
	public class EnergyBarDescriptorEditor : Editor {
		private EnergyBarDescriptor _energyBar;
		private List<GameObject> _deactivatedGos;
		private List<GameObject> _activatedGos;
		private bool _foundStandardBar = false;

		private void OnEnable() {
			_energyBar = (EnergyBarDescriptor)target;
		}

		public override void OnInspectorGUI() {
			EditorGUI.BeginChangeCheck();
			DrawDefaultInspector();
			if (EditorGUI.EndChangeCheck()) {
				_energyBar.bundleId = _energyBar.author + "." + _energyBar.name;
			}

			if (GUILayout.Button("Export")) {
				string path = EditorUtility.SaveFilePanel("Save Custom Energy Bar", "", _energyBar.name + ".energy", "energy");

				if (!string.IsNullOrWhiteSpace(path)) {
					string fileName = Path.GetFileName(path);
					string folderPath = Path.GetDirectoryName(path);

					foreach (EventManager em in _energyBar.GetComponentsInChildren<EventManager>()) {
						em.SerializeEvents();
					}
					_deactivatedGos = new List<GameObject>();
					_activatedGos = new List<GameObject>();
					_energyBar.batteryBars = new List<GameObject>();
					_energyBar.standardBar = null;
					_foundStandardBar = false;
					DeactivateBatteryBars(_energyBar.transform);
					if (!_foundStandardBar) {
						ReactivateBars();
						EditorUtility.DisplayDialog("Export Failed!", "StandardBar GameObject could not be found.", "OK");
						return;
					}

					Selection.activeObject = _energyBar.gameObject;
					EditorUtility.SetDirty(_energyBar);
					EditorSceneManager.MarkSceneDirty(_energyBar.gameObject.scene);
					EditorSceneManager.SaveScene(_energyBar.gameObject.scene);

					PrefabUtility.CreatePrefab("Assets/_CustomEnergyBar.prefab", _energyBar.gameObject as GameObject);
					AssetBundleBuild assetBundleBuild = default(AssetBundleBuild);
					assetBundleBuild.assetNames = new string[] {
						"Assets/_CustomEnergyBar.prefab"
					};
					assetBundleBuild.assetBundleName = fileName;

					BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
					BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;

					BuildPipeline.BuildAssetBundles(Application.temporaryCachePath, new AssetBundleBuild[] { assetBundleBuild }, BuildAssetBundleOptions.ForceRebuildAssetBundle , buildTarget);
					EditorPrefs.SetString("currentBuildingAssetBundlePath", folderPath);
					EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);

					AssetDatabase.DeleteAsset("Assets/_CustomEnergyBar.prefab");

					if (File.Exists(path)) {
						bool isDirectory = (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
						if (!isDirectory) File.Delete(path);
					}

					File.Move(Path.Combine(Application.temporaryCachePath, fileName), path);
					AssetDatabase.Refresh();
					EditorUtility.DisplayDialog("Export Successful!", "Export Successful!", "OK");

					ReactivateBars();
				} else {
					EditorUtility.DisplayDialog("Export Failed!", "Save path is invalid.", "OK");
				}
			}
		}

		public void DeactivateBatteryBars(Transform thisTransform) {
			foreach (Transform child in thisTransform) {
				DeactivateBatteryBars(child);
				if (child.name.Contains("LifeBar")) {
					if (child.gameObject.activeInHierarchy) {
						_deactivatedGos.Add(child.gameObject);
						child.gameObject.SetActive(false);
					}
					_energyBar.batteryBars.Add(child.gameObject);
				} else if (child.name == "StandardBar") {
					if (!child.gameObject.activeInHierarchy) {
						_activatedGos.Add(child.gameObject);
						child.gameObject.SetActive(true);
					}
					_energyBar.standardBar = child.gameObject;
					_foundStandardBar = true;
				}
			}
		}

		public void ReactivateBars() {
			foreach (GameObject go in _deactivatedGos) {
				go.SetActive(true);
			}
			foreach (GameObject go in _activatedGos) {
				go.SetActive(false);
			}
		}
	}
}