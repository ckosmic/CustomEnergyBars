using HMUI;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;
using TMPro;
using Zenject;

namespace CustomEnergyBar.Settings.UI
{
	[HotReload(RelativePathToLayout = @"Views\energyBarList.bsml")]
	[ViewDefinition("CustomEnergyBar.Settings.UI.Views.energyBarList.bsml")]
	internal class EnergyBarListViewController : BSMLAutomaticViewController
	{
		private PreviewEnergyBarManager _previewEnergyBarManager;
		private EnergyLoader _energyLoader;

		private bool _isGeneratingPreview = false;
		internal GameObject _previewGo;

		[UIComponent("energyBarList")]
		public CustomListTableData customListTableData = null;
		[UIComponent("description")]
		public TextMeshProUGUI description;

		[UIValue("description-text")]
		public string descriptionText = "No description provided";

		public EnergyBarPreviewViewController energyBarPreviewViewController;

		[Inject]
		internal void Construct(PreviewEnergyBarManager previewEnergyBarManager, EnergyLoader energyLoader) {
			_previewEnergyBarManager = previewEnergyBarManager;
			_energyLoader = energyLoader;
		}

		[UIAction("energyBarSelect")]
		public void Select(TableView view, int row) {
			_energyLoader.SelectedEnergyBar = row;
			EnergyBarDescriptor descriptor = _energyLoader.CustomEnergyBars[row].descriptor;
			Plugin.Settings.Selected = descriptor.bundleId;
			if (!string.IsNullOrWhiteSpace(descriptor.description)) {
				description.text = descriptor.description;
			}
			GeneratePreview(row);
		}

		[UIAction("reloadEnergyBars")]
		public void ReloadEnergyBars() {
			_energyLoader.Reload();
			SetupList();
			Select(customListTableData.tableView, _energyLoader.SelectedEnergyBar);
		}

		[UIAction("#post-parse")]
		public void SetupList() {
			customListTableData.data.Clear();

			foreach (EnergyBar energyBar in _energyLoader.CustomEnergyBars) {
				CustomListTableData.CustomCellInfo customCellInfo = new CustomListTableData.CustomCellInfo(energyBar.descriptor.name, energyBar.descriptor.author, energyBar.descriptor.icon);
				customListTableData.data.Add(customCellInfo);
			}

			customListTableData.tableView.ReloadData();
			int selectedEnergyBar = _energyLoader.SelectedEnergyBar;

			customListTableData.tableView.ScrollToCellWithIdx(selectedEnergyBar, TableView.ScrollPositionType.Beginning, false);
			customListTableData.tableView.SelectCellWithIdx(selectedEnergyBar);
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
			base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

			if (!_previewGo) {
				_previewGo = new GameObject();
				_previewGo.transform.position = new Vector3(3.25f, 0.9f, 1.7f);
				_previewGo.transform.Rotate(0.0f, 66.0f, 0.0f);
				_previewGo.transform.localScale = Vector3.one * 0.25f;
				_previewGo.name = "EnergyBarPreviewContainer";
			}

			int selectedEnergyBar = _energyLoader.SelectedEnergyBar;
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

				EnergyBar energyBar = _energyLoader.CustomEnergyBars[selected];
				if (energyBar.descriptor.bundleId == "defaultEnergyBar") {
					energyBarPreviewViewController.ShowMessage("No preview available");
				} else {
					GameObject prefab = energyBar.energyBarPrefab;
					if (energyBar != null && prefab != null) {
						GameObject go = Instantiate(prefab, _previewGo.transform.position, _previewGo.transform.rotation);
						go.transform.SetParent(_previewGo.transform);
						_previewEnergyBarManager.StartSimulation(go);
					}
					energyBarPreviewViewController.ShowMessage("");
				}
				_isGeneratingPreview = false;
			}
		}

		internal void ClearEnergyBar() {
			_previewEnergyBarManager?.StopSimulation();
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
