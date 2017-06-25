using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using TicTacToe.Model;

namespace TicTacToe.ViewModels
{
    public class PlayerListViewModel : BindableBase
    {
        public IPlayer Player { get; private set; }

        public string Name { get { return this.Player.Name; } }
        public double Wins { get { return this.Player.ScoreStats.Wins; } }
        public double Draws { get { return this.Player.ScoreStats.Draws; } }
        public double Losses { get { return this.Player.ScoreStats.Losses; } }

        public LinearGradientBrush PlayerColour
        {
            get
            {
                var gsc = new GradientStopCollection();
                if (this.Player is HumanPlayer)
                {
                    gsc.Add(new GradientStop(Colors.IndianRed, 0));
                    gsc.Add(new GradientStop(Colors.Red, 1));
                }
                else
                {
                    gsc.Add(new GradientStop(Colors.CornflowerBlue, 0));
                    gsc.Add(new GradientStop(Colors.Blue, 1));
                }
                return new LinearGradientBrush(gsc);
            }


        }


        public string PlayerType
        {
            get
            {
                return this.Player is HumanPlayer ? "Human" : "Computer";
            }
        }

        public string PlayerSummary
        {
            get
            {
                return "W: " + this.Player.ScoreStats.Wins + " / D: " + this.Player.ScoreStats.Draws + " / L: " + this.Player.ScoreStats.Losses;
            }
        }

        public PlayerListViewModel(IPlayer player)
        {
            this.Player = player;
            this.Player.PropertyChanged += this.PlayerPropertyChanged;
        }

        protected void PlayerPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "HasWon")
            {
//                this.RaisePropertyChanged("PlayerSummary");
                this.RaisePropertyChanged("Wins");
                this.RaisePropertyChanged("Losses");
                this.RaisePropertyChanged("Draws");
            }
        }
    }
}
