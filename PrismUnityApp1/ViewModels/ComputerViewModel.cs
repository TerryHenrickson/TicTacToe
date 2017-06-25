using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TicTacToe.Model;

namespace TicTacToe.ViewModels
{
    public class ComputerViewModel : PlayerViewModel
    {
        private ColouredBoardViewModel colouredBoardViewModel;

        public List<string> RateOfLearningOptions { get; set; }
        public List<string> InnitiativeOptions { get; set; }

        public int SelectedRateOfLearning
        {
            get
            {
                if (this.player == null)
                {
                    return 0;
                }
                return (int)(((ComputerPlayer)this.player).RateOfLearning*20);
            }

            set
            {
                ((ComputerPlayer)this.player).RateOfLearning = value / 20d;
            }
        }

        public int SelectedInnitiative
        {
            get
            {
                if (this.player == null)
                {
                    return 0;
                }
                return (int)(((ComputerPlayer)this.player).Innitiative * 20);
            }

            set
            {
                ((ComputerPlayer)this.player).Innitiative = value / 20d;
            }
        }


        public ColouredBoardViewModel ColouredBoardViewModel
        {
            get
            {
                return this.colouredBoardViewModel;
            }

            set
            {
                this.SetProperty(ref this.colouredBoardViewModel, value);
            }
        }

        private ComputerPlayer ComputerPlayer { get { return (ComputerPlayer)this.player; } }

        public ComputerViewModel() : base()
        {
            this.RateOfLearningOptions = new List<string>() { "0%", "5%", "10%", "15%", "20%" };
            this.InnitiativeOptions = new List<string>() { "0%", "5%", "10%", "15%", "20%" };
        }

        protected override void PlayerPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            base.PlayerPropertyChanged(sender, propertyChangedEventArgs);

            if (propertyChangedEventArgs.PropertyName == "AvailableMoveScores")
            {
                this.ColouredBoardViewModel = new ColouredBoardViewModel(((ComputerPlayer)sender).AvailableMoveScores);
            }
        }

        public override void SetBoardPosition(Board board, Mark previousMark)
        {
            this.player.SetBoardPosition(board, previousMark);
        }

        //public double RateOfLearning
        //{
        //    get
        //    {
        //        if (this.ComputerPlayer == null) { return 0; }
        //        return this.ComputerPlayer.RateOfLearning * 20;
        //    }
        //    set
        //    {
        //        if (this.ComputerPlayer == null) { return; }
        //        this.ComputerPlayer.RateOfLearning = value / 20;
        //    }
        //}

        public int ThinkingSpeed
        {
            get
            {
                if (this.ComputerPlayer == null) { return 0; }
                return 10 - this.ComputerPlayer.ThinkingSpeed;
            }
            set
            {
                if (this.ComputerPlayer == null) { return; }
                this.ComputerPlayer.ThinkingSpeed = 10 - value;
            }
        }

        //public double Innitiative
        //{
        //    get
        //    {
        //        if (this.ComputerPlayer == null) { return 0; }
        //        return this.ComputerPlayer.Innitiative * 20;
        //    }
        //    set
        //    {
        //        if (this.ComputerPlayer == null) { return; }
        //        this.ComputerPlayer.Innitiative = value / 20;
        //    }
        //}

        public override void SetPlayer(IPlayer player)
        {
            base.SetPlayer(player);
            this.UpdateViewModelSettings();
        }

        protected override void UpdateViewModelSettings()
        {
            base.UpdateViewModelSettings();
            this.RaisePropertyChanged("SelectedRateOfLearning");
            this.RaisePropertyChanged("ThinkingSpeed");
            this.RaisePropertyChanged("SelectedInnitiative");
        }
 

    }
}