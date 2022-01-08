using IPA.Config.Stores;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace CustomEnergyBar.Settings
{
	internal class SettingsStore
	{

		public string Selected = "defaultEnergyBar";
		public bool AllowSFX = true;

	}
}
