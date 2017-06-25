using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model.DataObjects;

namespace TicTacToe.Model
{
    public abstract class Player : IPlayer
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected GameEndType? hasWon;
        protected ScoreStats scoreStats;
        protected Board board;

        public virtual void Reset()
        {
            this.HasWon = null;
        }

        public virtual void SetBoardPosition(Board board, Mark mark)
        {
            this.Board = board;

            var winner = this.Board.GetWinnerIfExists();
            if(winner != Mark.None)
            {
                this.HasWon = this.GetGameEndTypeAndIncrementScoreStats(winner);
            }
        }

        public virtual int? GetMove(Mark mark)
        {
            return null;
        }

        protected GameEndType GetGameEndTypeAndIncrementScoreStats(Mark winner)
        {
            if (((winner & Mark.Cross) !=0) && ((winner & Mark.Nought) !=0))
            {
                this.ScoreStats.Draws += 1;
                this.OnPropertyChanged("ScoreStats");
                return GameEndType.Drawn;
            }
            if ((winner & this.Mark) != 0)
            {
                this.ScoreStats.Wins += 1;
                this.OnPropertyChanged("ScoreStats");
                return GameEndType.Won;
            }
            else
            {
                this.ScoreStats.Losses += 1;
                this.OnPropertyChanged("ScoreStats");
                return GameEndType.Lost;
            }
        }

        public Player()
        {
            this.ScoreStats = new ScoreStats();
        }

        public GameEndType? HasWon
        {
            get
            {
                return this.hasWon;
            }

            set
            {
                this.hasWon = value;
                this.OnPropertyChanged();
            }
        }

        public string Name { get; set; }
        public Mark Mark { get; set; }
        public Game Game { get; set; }

        public bool CanIPlayMark(Mark markToPlay)
        {
            return (markToPlay & this.Mark) != 0;
        }

        public Board Board
        {
            get
            {
                return this.board;
            }

            protected set
            {
                this.board = value;
                this.OnPropertyChanged();
            }
        }

        public ScoreStats ScoreStats {
            get
            {
                return this.scoreStats;
            }

            set
            {
                this.scoreStats = value;
                this.OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
