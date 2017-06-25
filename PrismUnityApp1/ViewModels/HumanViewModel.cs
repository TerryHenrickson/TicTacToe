using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TicTacToe.Model;

namespace TicTacToe.ViewModels
{
    public class HumanViewModel : PlayerViewModel
    {
        public DelegateCommand<string> PlayMarkCommand { get; set; }

        private Mark nextMarkToPlay;

        public HumanViewModel() : base()
        {
            this.PlayMarkCommand = new DelegateCommand<string>(this.PlayMark, this.CanPlayMark).ObservesProperty(() => this.Board);
            this.HideGameEndMessages();
        }

        public override void Reset()
        {
            base.Reset();
            this.HideGameEndMessages();
        }

        private void HideGameEndMessages()
        {
            this.WinVisibility = Visibility.Hidden;
            this.LossVisibility = Visibility.Hidden;
            this.DrawVisibility = Visibility.Hidden;
        }

        public override GameEndType? HasWon
        {
            get
            {
                return this.hasWon;
            }

            set
            {
                this.SetProperty(ref this.hasWon, value);
                switch (this.hasWon)
                {
                    case GameEndType.Won:
                        this.WinVisibility = Visibility.Visible;
                        this.LossVisibility = Visibility.Hidden;
                        this.DrawVisibility = Visibility.Hidden;
                        break;
                    case GameEndType.Lost:
                        this.WinVisibility = Visibility.Hidden;
                        this.LossVisibility = Visibility.Visible;
                        this.DrawVisibility = Visibility.Hidden;
                        break;
                    case GameEndType.Drawn:
                        this.WinVisibility = Visibility.Hidden;
                        this.LossVisibility = Visibility.Hidden;
                        this.DrawVisibility = Visibility.Visible;
                        break;
                }
            }
        }


        private void PlayMark(string position)
        {
            Task.Run(() =>
            {
                if (this.player.CanIPlayMark(this.nextMarkToPlay) && this.Board.CanPlayInPosition(int.Parse(position)))
                {
                    this.game.HumanMove(this.nextMarkToPlay, int.Parse(position));
                }
            }); 
        }

        private bool CanPlayMark(string position)
        {
            return true;

            //if (this.Board == null)
            //{
            //    return false;
            //}
            //return this.Board.CanPlayInPosition(int.Parse(position));
        }

        public override void SetBoardPosition(Board board, Mark lastMarkPlayed)
        {
            this.player.SetBoardPosition(board, Mark.None);
            this.nextMarkToPlay = lastMarkPlayed == Mark.Cross ? Mark.Nought : Mark.Cross;
        }

        private Visibility winVisibility;

        public Visibility WinVisibility
        {
            get
            {
                return this.winVisibility;
            }

            set
            {
                this.SetProperty(ref winVisibility, value);
            }
        }

        private Visibility lossVisibility;

        public Visibility LossVisibility
        {
            get
            {
                return this.lossVisibility;
            }

            set
            {
                this.SetProperty(ref lossVisibility, value);
            }
        }

        private Visibility drawVisibility;

        public Visibility DrawVisibility
        {
            get
            {
                return this.drawVisibility;
            }

            set
            {
                this.SetProperty(ref drawVisibility, value);
            }
        }



    }
}
