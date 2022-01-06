using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using TMPro;

namespace CustomEnergyBar.Settings.UI
{
	[HotReload(RelativePathToLayout = @"Views\energyBarPreview.bsml")]
	[ViewDefinition("CustomEnergyBar.Settings.UI.Views.energyBarPreview.bsml")]
	internal class EnergyBarPreviewViewController : BSMLAutomaticViewController
	{
		[UIComponent("message-text")]
		public TextMeshProUGUI messageText;

		public void ShowMessage(string msg) {
			messageText.text = msg;
			messageText.gameObject.SetActive(!string.IsNullOrEmpty(msg));
		}
	}
}
