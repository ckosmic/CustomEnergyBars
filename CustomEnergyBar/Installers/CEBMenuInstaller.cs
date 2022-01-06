using CustomEnergyBar.Settings.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace CustomEnergyBar.Installers
{
    internal class CEBMenuInstaller : Installer<CEBMenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PreviewEnergyBarManager>().AsSingle();

            Container.Bind<EnergyBarListViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<EnergyBarPreviewViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<SettingsViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<CEBFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();

            Container.BindInterfacesTo<SettingsUI>().AsSingle();
        }
    }
}
