using IPA.Config.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace CustomEnergyBar.Settings
{
	internal class SettingsStore
	{

		public string Selected = "defaultEnergyBar";
		public bool AllowSFX = true;

	}
}
