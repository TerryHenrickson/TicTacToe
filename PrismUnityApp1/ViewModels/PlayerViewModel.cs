using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TicTacToe.Model;

namespace TicTacToe.ViewModels
{
    public abstract class PlayerViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand RemovePlayerFromSideCommand { get; set; }

        public List<string> MarkOptions { get; set; }

        private SimpleBoardViewModel simpleBoardViewModel;

        public SimpleBoardViewModel Board
        {
            get
            {
                return this.simpleBoardViewModel;
            }

            set
            {
                this.SetProperty(ref this.simpleBoardViewModel, value);
            }
        }

        protected GameEndType? hasWon;

        public virtual GameEndType? HasWon
        {
            get
            {
                return this.hasWon;
            }

            set
            {
                this.SetProperty(ref this.hasWon, value);
            }
        }

        public string Name
        {
            get
            {
                if (this.player == null)
                {
                    return string.Empty;
                }
                return this.player.Name;
            }
        }

        protected IPlayer player;
        protected GameViewModel game;
        public abstract void SetBoardPosition(Board board, Mark previousMark);

        public virtual int? GetMove(Mark mark)
        {
            return this.player.GetMove(mark);
        } 

        public PlayerViewModel()
        {
            this.MarkOptions = new List<string>() { "Cross", "Nought", "Both" };
            this.RemovePlayerFromSideCommand = new DelegateCommand(this.RemovePlayerFromSide);
        }

        public virtual void SetPlayer(IPlayer player)
        {
            this.player = player;
            this.player.PropertyChanged += this.PlayerPropertyChanged;
            this.UpdateViewModelSettings();
            this.UpdateViewModelScores();
        }

        public bool IsMyPlayer(IPlayer player)
        {
            return player == this.player;
        }

        private void RemovePlayerFromSide()
        {
            this.game.RemovePlayer(this);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.SetPlayer((IPlayer)navigationContext.Parameters["player"]);
            this.game = (GameViewModel)navigationContext.Parameters["game"];
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;// throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if(this.player != null)
            {
                this.player.PropertyChanged -= this.PlayerPropertyChanged;
                this.player = null;
            }
        }

        protected virtual void PlayerPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if(propertyChangedEventArgs.PropertyName == "ScoreStats")
            {
                this.UpdateViewModelScores();
            }
            if (propertyChangedEventArgs.PropertyName == "Board")
            {
                this.Board = new SimpleBoardViewModel(((IPlayer)sender).Board);
                this.RaisePropertyChanged("Board");
            }
            if (propertyChangedEventArgs.PropertyName == "HasWon")
            {
                this.HasWon = this.player.HasWon;
            }
        }

        protected virtual void UpdateViewModelSettings()
        {
            this.RaisePropertyChanged("Name");
            this.RaisePropertyChanged("SelectedMark");
        }

        protected void UpdateViewModelScores()
        {
            this.RaisePropertyChanged("Wins");
            this.RaisePropertyChanged("Draws");
            this.RaisePropertyChanged("Losses");
        }

        public int SelectedMark
        {
            get
            {
                if(this.player == null)
                {
                    return 0;
                }
                return (int)(this.player.Mark-1);
            }

            set
            {
                this.player.Mark = (Mark)(value+1);
            }
        }

        public long Wins
        {
            get
            {
                if(this.player == null) { return 0; }
                return this.player.ScoreStats.Wins;
            }
        }

        public long Draws
        {
            get
            {
                if (this.player == null) { return 0; }
                return this.player.ScoreStats.Draws;
            }
        }

        public long Losses
        {
            get
            {
                if (this.player == null) { return 0; }
                return this.player.ScoreStats.Losses;
            }
        }

        public virtual void Reset()
        {
            if (this.player == null) { return; }
            this.player.Reset();
        }
    }
}
