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
		public void Init(IPALogger logger, Config config, Zenjector zenject) {
			Log = logger;

			Settings = config.Generated<SettingsStore>();
			
			zenject.Install<CEBAppInstaller>(Location.App);
			zenject.Install<CEBMenuInstaller>(Location.Menu);
			zenject.Install<CEBGameInstaller>(Location.Player);
		}
	}
}
