using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace CustomEnergyBar.Settings.UI
{
	internal class EnergyBarPreviewViewController : BSMLResourceViewController
	{
		public override string ResourceName => "CustomEnergyBar.Settings.UI.Views.energyBarPreview.bsml";

		[UIComponent("message-text")]
		public TextMeshProUGUI messageText;

		public void ShowMessage(string msg) {
			messageText.text = msg;
			messageText.gameObject.SetActive(!string.IsNullOrEmpty(msg));
		}
	}
}
