using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using CustomEnergyBar.Settings;
using SiraUtil.Zenject;
using CustomEnergyBar.Installers;

namespace CustomEnergyBar
{

	[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
	public class Plugin
	{
		internal static IPALogger Log { get; private set; }
		internal static SettingsStore Settings { get; private set; }

		[Init]
		/// <summary>
		/// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
		/// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
		/// Only use [Init] with one Constructor.
		/// </summary>
		public void Init(IPALogger logger, Config config, Zenjector zenject) {
			Log = logger;

			Settings = config.Generated<SettingsStore>();

			EnergyLoader.Load();

			zenject.Expose<GameEnergyUIPanel>("CEB_GameEnergyUIPanel");
			zenject.Install<CEBMenuInstaller>(Location.Menu);
			zenject.Install<CEBGameInstaller>(Location.Player);
		}
	}
}
