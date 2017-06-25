using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model;
using TicTacToe.Views;

namespace TicTacToe
{
    public class TicTacToeModule : IModule
    {
        private readonly IUnityContainer unityContainer;

        private readonly IRegionManager regionManager;


        public void Initialize()
        {
            this.unityContainer.RegisterType<IPlayerFactory, PlayerFactory>();

            this.regionManager.RegisterViewWithRegion(Constants.LeftPlayerRegion, typeof(ComputerView));
            this.regionManager.RegisterViewWithRegion(Constants.LeftPlayerRegion, typeof(HumanView));
            this.regionManager.RegisterViewWithRegion(Constants.LeftPlayerRegion, typeof(EmptyView));

            this.regionManager.RegisterViewWithRegion(Constants.RightPlayerRegion, typeof(ComputerView));
            this.regionManager.RegisterViewWithRegion(Constants.RightPlayerRegion, typeof(HumanView));
            this.regionManager.RegisterViewWithRegion(Constants.RightPlayerRegion, typeof(EmptyView));

            this.regionManager.RegisterViewWithRegion(Constants.StatisticsRegion, typeof(StatisticsView));
            this.regionManager.RegisterViewWithRegion(Constants.PlayersRegion, typeof(PlayersView));
            this.regionManager.RegisterViewWithRegion(Constants.GameRegion, typeof(GameView));



        }

        public TicTacToeModule(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
        }
    }
}
