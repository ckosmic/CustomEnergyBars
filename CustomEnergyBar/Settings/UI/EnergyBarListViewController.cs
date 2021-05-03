using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMUI;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;
using TMPro;

namespace CustomEnergyBar.Settings.UI
{
	internal class EnergyBarListViewController : BSMLResourceViewController
	{
		public override string ResourceName => "CustomEnergyBar.Settings.UI.Views.energyBarList.bsml";

		private bool _isGeneratingPreview = false;
		internal GameObject _previewGo;

		[UIComponent("energyBarList")]
		public CustomListTableData customListTableData = null;
		[UIComponent("description")]
		public TextMeshProUGUI description;

		[UIValue("description-text")]
		public string descriptionText = "No description provided";

		public EnergyBarPreviewViewController energyBarPreviewViewController;

		[UIAction("energyBarSelect")]
		public void Select(TableView view, int row) {
			EnergyLoader.SelectedEnergyBar = row;
			EnergyBarDescriptor descriptor = EnergyLoader.CustomEnergyBars[row].descriptor;
			Plugin.Settings.Selected = descriptor.bundleId;
			if (!string.IsNullOrWhiteSpace(descriptor.description)) {
				description.text = descriptor.description;
			}
			GeneratePreview(row);
		}

		[UIAction("reloadEnergyBars")]
		public void ReloadEnergyBars() {
			EnergyLoader.Reload();
			Select(customListTableData.tableView, EnergyLoader.SelectedEnergyBar);
		}

		[UIAction("#post-parse")]
		public void SetupList() {
			customListTableData.data.Clear();

			foreach (EnergyBar energyBar in EnergyLoader.CustomEnergyBars) {
				CustomListTableData.CustomCellInfo customCellInfo = new CustomListTableData.CustomCellInfo(energyBar.descriptor.name, energyBar.descriptor.author, energyBar.descriptor.icon);
				customListTableData.data.Add(customCellInfo);
			}

			customListTableData.tableView.ReloadData();
			int selectedEnergyBar = EnergyLoader.SelectedEnergyBar;

			customListTableData.tableView.ScrollToCellWithIdx(selectedEnergyBar, TableView.ScrollPositionType.Beginning, false);
			customListTableData.tableView.SelectCellWithIdx(selectedEnergyBar);
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
			base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

			if (!_previewGo) {
				_previewGo = new GameObject();
				_previewGo.transform.position = new Vector3(3.05f, 0.9f, 2.0f);
				_previewGo.transform.Rotate(0.0f, 60.0f, 0.0f);
				_previewGo.transform.localScale = Vector3.one * 0.25f;
				_previewGo.name = "EnergyBarPreviewContainer";
			}

			int selectedEnergyBar = EnergyLoader.SelectedEnergyBar;
			customListTableData.tableView.SelectCellWithIdx(selectedEnergyBar);
			Select(customListTableData.tableView, selectedEnergyBar);
		}

		protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling) {
			base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
			ClearPreview();
		}

		internal void GeneratePreview(int selected) {
			if (!_isGeneratingPreview) {
				_isGeneratingPreview = true;

				ClearEnergyBar();

				EnergyBar energyBar = EnergyLoader.CustomEnergyBars[selected];
				if (energyBar.descriptor.bundleId == "defaultEnergyBar") {
					energyBarPreviewViewController.ShowMessage("No preview available");
				} else {
					GameObject prefab = energyBar.energyBarPrefab;
					if (energyBar != null && prefab != null) {
						GameObject go = Instantiate(prefab, _previewGo.transform.position, _previewGo.transform.rotation);
						go.transform.SetParent(_previewGo.transform);
						PreviewEnergyBarManager.Instance.StartSimulation(go);
					}
					energyBarPreviewViewController.ShowMessage("");
				}
				_isGeneratingPreview = false;
			}
		}

		internal void ClearEnergyBar() {
			PreviewEnergyBarManager.Instance?.StopSimulation();
			foreach (Transform child in _previewGo.transform) {
				Destroy(child.gameObject);
			}
		}

		private void ClearPreview() {
			ClearEnergyBar();
			Destroy(_previewGo);
		}
	}
}
