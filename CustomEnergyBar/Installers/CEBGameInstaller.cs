using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace CustomEnergyBar.Installers
{
    internal class CEBGameInstaller : Installer<CEBGameInstaller>
    {
        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<EnergyBarManager>().AsSingle();
        }
    }
}
