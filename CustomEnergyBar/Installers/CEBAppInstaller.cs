using CustomEnergyBar.API;
using Zenject;

namespace CustomEnergyBar.Installers
{
    internal class CEBAppInstaller : Installer<CEBAppInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<EnergyLoader>().AsSingle();
            Container.Bind<CEBAPI>().AsSingle();
        }
    }
}
