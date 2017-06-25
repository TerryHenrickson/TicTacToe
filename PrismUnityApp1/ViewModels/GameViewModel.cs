using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Model;
using TicTacToe.Views;

namespace TicTacToe.ViewModels
{
    public class GameViewModel : BindableBase
    {
        private PlayerViewModel leftPlayer;
        private PlayerViewModel rightPlayer;

        private CancellationTokenSource cancellationTokenSource;
        private bool gameRunning;

        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand StopCommand { get; set; }

        private long numberOfGames;

        public long NumberOfGames
        {
            get { return this.numberOfGames; }
            set
            {
                this.SetProperty(ref numberOfGames, value);
            }
        }

        private Board board;
        private Mark mark;

        private IRegionManager regionManager;

        private IEventAggregator eventAggregator;

        public GameViewModel(IRegionManager regionManager, IEventAggregator eventAggregator )
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<UsePlayerEvent>().Subscribe(this.SetPlayer);
            this.StartCommand = new DelegateCommand(this.StartGame);
            this.StopCommand = new DelegateCommand(this.StopGame);

            regionManager.RequestNavigate("LeftPlayerRegion", new Uri("EmptyView", UriKind.Relative));
            regionManager.RequestNavigate("RightPlayerRegion", new Uri("EmptyView", UriKind.Relative));

            // Initial position
            this.board = new Board(0);
            this.mark = Mark.Cross;
        }

        private void SetPlayer(Tuple<SideType, IPlayer> sideAndPlayer)
        {
            if ((this.leftPlayer != null && this.leftPlayer.IsMyPlayer(sideAndPlayer.Item2)) 
                || (this.rightPlayer != null && this.rightPlayer.IsMyPlayer(sideAndPlayer.Item2)))
            {
                return;
            }

            if (sideAndPlayer.Item1 == SideType.Left)
            {
                this.SetLeftPlayer(sideAndPlayer.Item2);
            }
            else if (sideAndPlayer.Item1 == SideType.Right)
            {
                this.SetRightPlayer(sideAndPlayer.Item2);
            }
        }

        public void RemovePlayer(PlayerViewModel player)
        {
            if (player == this.leftPlayer)
            {
                regionManager.RequestNavigate("LeftPlayerRegion", new Uri("EmptyView", UriKind.Relative));
                this.leftPlayer = null;
            }
            else if (player == this.rightPlayer)
            {
                regionManager.RequestNavigate("RightPlayerRegion", new Uri("EmptyView", UriKind.Relative));
                this.rightPlayer = null;
            }
        }

        public void SetLeftPlayer(IPlayer player)
        {
            var parameters = new NavigationParameters { { "player", player }, { "game", this } };

            if (player is HumanPlayer)
            {
                regionManager.RequestNavigate("LeftPlayerRegion", new Uri("HumanView", UriKind.Relative), parameters);
                this.leftPlayer = (PlayerViewModel)((HumanView)regionManager.Regions["LeftPlayerRegion"].ActiveViews.FirstOrDefault()).DataContext;
            }
            else if(player is ComputerPlayer)
            {
                regionManager.RequestNavigate("LeftPlayerRegion", new Uri("ComputerView", UriKind.Relative), parameters);
                this.leftPlayer = (PlayerViewModel)((ComputerView)regionManager.Regions["LeftPlayerRegion"].ActiveViews.FirstOrDefault()).DataContext;
            }

            this.leftPlayer.SetBoardPosition(this.board, Mark.None); //  update with the latest board 
        }

        public void SetRightPlayer(IPlayer player)
        {
            var parameters = new NavigationParameters { { "player", player }, { "game", this } };

            if (player is HumanPlayer)
            {
                regionManager.RequestNavigate("RightPlayerRegion", new Uri("HumanView", UriKind.Relative), parameters);
                this.rightPlayer = (PlayerViewModel)((HumanView)regionManager.Regions["RightPlayerRegion"].ActiveViews.FirstOrDefault()).DataContext;
            }
            else if (player is ComputerPlayer)
            {
                regionManager.RequestNavigate("RightPlayerRegion", new Uri("ComputerView", UriKind.Relative), parameters);
                this.rightPlayer = (PlayerViewModel)((ComputerView)regionManager.Regions["RightPlayerRegion"].ActiveViews.FirstOrDefault()).DataContext;
            }

            this.rightPlayer.SetBoardPosition(this.board, Mark.None); //  update with the latest board 
        }

        public void StartGame()
        {
            if(this.leftPlayer == null && this.rightPlayer == null)
            {
                return;
            }

            this.cancellationTokenSource = new CancellationTokenSource();
            this.InitialiseGame();
            this.LoopGames();
        }


        private void InitialiseGame()
        {
            if (this.leftPlayer != null) { this.leftPlayer.Reset(); };
            if (this.rightPlayer != null) { this.rightPlayer.Reset(); };
            this.board = new Board(0);
            this.SendBoardToPlayers(this.board, Mark.None);
            this.mark = Mark.Cross;
            this.gameRunning = true;
            this.NumberOfGames += 1;
        }

        private async void LoopGames()
        {
            if (this.leftPlayer is HumanViewModel || this.rightPlayer is HumanViewModel)
            {
                this.LoopWhenAtLeastOneHumanPlayerInvolved();
            }
            else
            {

                await Task.Run(() => this.LoopWhenOnlyComputerPlayerInvolved(cancellationTokenSource.Token));
            }
        }

        private int? GetMoveFromPlayers(Mark mark)
        {
            int? move = null;

            if (this.leftPlayer != null)
            {
                move = this.leftPlayer.GetMove(mark);
            }
            if (this.rightPlayer != null && move == null)
            {
                move = this.rightPlayer.GetMove(mark);
            }

            return move;
        }


        private void SendBoardToPlayers(Board loopBoard, Mark loopMark)
        {
            if (this.leftPlayer != null)
            {
                this.leftPlayer.SetBoardPosition(loopBoard, loopMark);
            }
            if (this.rightPlayer != null)
            {
                this.rightPlayer.SetBoardPosition(loopBoard, loopMark);
            }   
        }

        public void StopGame()
        {
            this.cancellationTokenSource.Cancel();
            this.gameRunning = false;
        }

        private void LoopWhenOnlyComputerPlayerInvolved(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {             
                var move = this.GetMoveFromPlayers(this.mark);

                if (move == null)
                {
                    break;
                }

                this.board = this.board.AddMarkToLocation(this.mark, (int)move);

                this.SendBoardToPlayers(this.board, this.mark);
                
                var winner = this.board.GetWinnerIfExists();

                if (winner != Mark.None)
                {
                    this.eventAggregator.GetEvent<GameEndEvent>().Publish(winner);
                    this.InitialiseGame();
                    continue;
                }

                this.mark = this.mark == Mark.Cross ? Mark.Nought : Mark.Cross;
            }
        }


        public void LoopWhenAtLeastOneHumanPlayerInvolved()
        {
            while (this.gameRunning)
            {
                var move = this.GetMoveFromPlayers(this.mark);

                if (move == null)
                {
                    break;
                }

                this.board = this.board.AddMarkToLocation(this.mark, (int)move);

                this.SendBoardToPlayers(this.board, this.mark);

                var winner = this.board.GetWinnerIfExists();

                if (winner != Mark.None)
                {
                    this.eventAggregator.GetEvent<GameEndEvent>().Publish(winner);
                    break;
                }

                this.mark = this.mark == Mark.Cross ? Mark.Nought : Mark.Cross;

            }

        }

        public void HumanMove(Mark mark, int move)
        {
            if (!this.gameRunning)
            {
                return;
            }

            this.board = this.board.AddMarkToLocation(mark, (int)move);
            this.SendBoardToPlayers(this.board, mark);

            var winner = this.board.GetWinnerIfExists();
            if (winner != Mark.None)
            {
                this.eventAggregator.GetEvent<GameEndEvent>().Publish(winner);
                return;
            }

            this.mark = this.mark == Mark.Cross ? Mark.Nought : Mark.Cross;
            this.LoopWhenAtLeastOneHumanPlayerInvolved();
        }

    }
}
