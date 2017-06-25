using Microsoft.Practices.Unity;
using Prism.Unity;
using TicTacToe.Views;
using System.Windows;
using Prism.Modularity;

namespace TicTacToe
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var moduleCatalog = (ModuleCatalog)this.ModuleCatalog;

            moduleCatalog.AddModule(typeof(TicTacToeModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            //Container.RegisterTypeForNavigation<StatisticsView>("StatisticsRegion");
        }
    }
}
