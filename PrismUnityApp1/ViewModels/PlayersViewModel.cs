using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using TicTacToe.Model;

namespace TicTacToe.ViewModels
{
    public class PlayersViewModel : BindableBase
    {
        public ObservableCollection<PlayerListViewModel> PlayerListViewModels { get; set; }

        private PlayerListViewModel selectedPlayerViewModel;

        private IEventAggregator eventAggregator;
        private IPlayerFactory playerFactory;

        public DelegateCommand AddHumanPlayerCommand { get; set; }
        public DelegateCommand AddComputerPlayerCommand { get; set; }
        public DelegateCommand<string> PutSlectedPlayerIntoGameCommand { get; set; }
        
        public PlayersViewModel(IEventAggregator eventAggregator, IPlayerFactory playerFactory)
        {
            this.eventAggregator = eventAggregator;
            this.playerFactory = playerFactory;

            this.PlayerListViewModels = new ObservableCollection<PlayerListViewModel>();
            this.AddHumanPlayerCommand = new DelegateCommand(this.AddHumanPlayer);
            this.AddComputerPlayerCommand = new DelegateCommand(this.AddComputerPlayer);
            this.PutSlectedPlayerIntoGameCommand = new DelegateCommand<string>(this.PutSlectedPlayerIntoGame);
        }

        public PlayerListViewModel SelectedPlayer
        {
            get
            {
                return this.selectedPlayerViewModel;
            }

            set
            {
                this.SetProperty(ref this.selectedPlayerViewModel, value);
            }
        }

        private void AddHumanPlayer()
        {
            this.AddPlayerViewModel(this.playerFactory.GetPlayer(PlayerType.Human));
        }

        private void AddComputerPlayer()
        {
            this.AddPlayerViewModel(this.playerFactory.GetPlayer(PlayerType.Computer));
        }

        private void DeleteSelectedPlayer()
        {
            if(this.SelectedPlayer != null)
            {
                this.PlayerListViewModels.Remove(this.selectedPlayerViewModel);
                this.SelectedPlayer = null;
                // should notify that player is deleted (since player maybe currently part of a game)
            }
        }

        private void PutSlectedPlayerIntoGame(string side)
        {
            if (this.selectedPlayerViewModel == null)
            {
                return;
            }

            if (side == "Left")
            {
                this.MakeSelectedPlayerLeftPlayer();
            }
            else if(side == "Right")
            {
                this.MakeSelectedPlayerRightPlayer();
            }
        }

        private void MakeSelectedPlayerLeftPlayer()
        {
            this.eventAggregator.GetEvent<UsePlayerEvent>().Publish(new Tuple<SideType, IPlayer>(SideType.Left, this.selectedPlayerViewModel.Player));
        }

        private void MakeSelectedPlayerRightPlayer()
        {
            this.eventAggregator.GetEvent<UsePlayerEvent>().Publish(new Tuple<SideType, IPlayer>(SideType.Right, this.selectedPlayerViewModel.Player));
        }


        private void AddPlayerViewModel(IPlayer player)
        {
            var playerListViewModel = new PlayerListViewModel(player);
            this.PlayerListViewModels.Add(playerListViewModel);
            playerListViewModel.PropertyChanged += PlayerListViewModel_PropertyChanged;
            this.SelectedPlayer = playerListViewModel;
        }

        private void PlayerListViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "PlayerSummary")
            {
                
                this.RaisePropertyChanged("PlayerListViewModels");
            }
        }
    }
}
