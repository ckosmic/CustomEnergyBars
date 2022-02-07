using Zenject;

namespace CustomEnergyBar.Installers
{
    internal class CEBGameInstaller : Installer<CEBGameInstaller>
    {
        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<EnergyBarManager>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}
