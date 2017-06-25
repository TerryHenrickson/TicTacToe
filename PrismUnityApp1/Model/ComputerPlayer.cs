using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public class ComputerPlayer : Player
    {
        private Dictionary<long, long> boardToUniqueEquivalent;
        private Dictionary<long, double> positionScores;
        private Dictionary<GridPoint, double> availableMoveScores;

        private long? lastXPosition;
        private long? lastOPosition;

        public double RateOfLearning { get; set; }
        public double Innitiative { get; set; }
        public int ThinkingSpeed { get; set; }
        public Random Random { get; private set; }

        public ComputerPlayer(Dictionary<long, long> boardToUniqueEquivalent, Dictionary<long, double> positionScores)
        {
            this.boardToUniqueEquivalent = boardToUniqueEquivalent;
            this.positionScores = positionScores;

            this.RateOfLearning = 0.05d;
            this.Innitiative = 0.05d;
            this.ThinkingSpeed = 3;

            this.Mark = Mark.Nought;
            this.Random = new Random(1);
        }

        public Dictionary<GridPoint, double> AvailableMoveScores
        {
            get
            {
                return this.availableMoveScores;
            }

            set
            {
                this.availableMoveScores = value;
                this.OnPropertyChanged();
            }
        }


        public override void Reset()
        {
            base.Reset();

            this.lastXPosition = null;
            this.lastOPosition = null;
            this.AvailableMoveScores = new Dictionary<GridPoint, double>();
        }
        
        public override void SetBoardPosition(Board board, Mark previousMark)
        {
            this.Board = board;

            var mark = previousMark == Mark.Cross ? Mark.Nought : Mark.Cross;

            // update position scores for move done by other player (if not itself)
            if (!this.CanIPlayMark(previousMark))
            {
                if (previousMark == Mark.Cross)
                {
                    if (this.lastXPosition != null)
                    {
                        this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] * (1-this.RateOfLearning) + this.positionScores[this.boardToUniqueEquivalent[board.GetBoardID()]] * this.RateOfLearning;
                    }
                    this.lastXPosition = board.GetBoardID();
                }
                else if (previousMark == Mark.Nought)
                {
                    if (this.lastOPosition != null)
                    {
                        this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] * (1 - this.RateOfLearning) + this.positionScores[this.boardToUniqueEquivalent[board.GetBoardID()]] * this.RateOfLearning;
                    }
                    this.lastOPosition = board.GetBoardID();
                }
            }

            var winner = this.Board.GetWinnerIfExists();

            if (winner != Mark.None)
            {
                if(this.CanIPlayMark(previousMark) && !this.CanIPlayMark(mark)) // record loss for human player
                {
                    var reward = ((winner & Mark.Cross) != 0 && (winner & Mark.Nought) != 0) ? 0.5 : 0;

                    if (mark == Mark.Cross)
                    {
                        this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] * (1 - this.RateOfLearning) + reward * this.RateOfLearning;
                    }
                    else if (mark == Mark.Nought)
                    {
                        this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] * (1 - this.RateOfLearning) + reward * this.RateOfLearning;
                    }
                }

                if (!this.CanIPlayMark(previousMark) && this.CanIPlayMark(mark)) // record loss for computer player
                {
                    var reward = ((winner & Mark.Cross) != 0 && (winner & Mark.Nought) != 0) ? 0.5 : 0;

                    if (mark == Mark.Cross)
                    {
                        this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] * (1 - this.RateOfLearning) + reward * this.RateOfLearning;
                    }
                    else if (mark == Mark.Nought)
                    {
                        this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] * (1 - this.RateOfLearning) + reward * this.RateOfLearning;
                    }
                }

                if (this.CanIPlayMark(previousMark) && this.CanIPlayMark(mark)) // record loss for computer player when playing itself
                {
                    var reward = ((winner & Mark.Cross) != 0 && (winner & Mark.Nought) != 0) ? 0.5 : 0;

                    if (mark == Mark.Cross)
                    {
                        this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] * (1 - this.RateOfLearning) + reward * this.RateOfLearning;
                    }
                    else if (mark == Mark.Nought)
                    {
                        this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] * (1 - this.RateOfLearning) + reward * this.RateOfLearning;
                    }
                }

                this.HasWon = this.GetGameEndTypeAndIncrementScoreStats(winner);

                if (this.ThinkingSpeed > 0)
                {
                    int milliseconds = 150 * this.ThinkingSpeed;
                    Thread.Sleep(milliseconds);
                }
            }


        }

        public override int? GetMove(Mark mark)
        {
            if (!this.CanIPlayMark(mark))
            {
                return null;
            }

            GridPoint move;
            bool innovativeMove;

            var availableMoves = this.board.GetEmptyPositions();
            this.AvailableMoveScores = availableMoves.ToDictionary(am => am, am => this.positionScores[this.boardToUniqueEquivalent[this.board.AddMarkToLocation(mark, (int)am.Position).GetBoardID()]]);

            var preferredMove = this.AvailableMoveScores.Aggregate((a, b) => a.Value > b.Value ? a : b);

            var remainingMoves = new Dictionary<GridPoint, double>(this.AvailableMoveScores);
            remainingMoves.Remove(preferredMove.Key);

            double randomNumber0To1 = this.Random.NextDouble();
            if (remainingMoves.Count > 0 && randomNumber0To1 < this.Innitiative)
            {
                move = remainingMoves.Keys.ElementAt(this.Random.Next(remainingMoves.Count));
                innovativeMove = true;
            }
            else
            {
                move = preferredMove.Key;
                innovativeMove = false;
            }

            var futureBoard = this.board.AddMarkToLocation(mark, move);

            if (mark == Mark.Cross)
            {
                if (this.lastXPosition != null && !innovativeMove)
                {
                    this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastXPosition]] * (1 - this.RateOfLearning) + this.positionScores[this.boardToUniqueEquivalent[futureBoard.GetBoardID()]] * this.RateOfLearning;
                }
                this.lastXPosition = futureBoard.GetBoardID();
            }
            else if (mark == Mark.Nought)
            {
                if (this.lastOPosition != null && !innovativeMove)
                {
                    this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] = this.positionScores[this.boardToUniqueEquivalent[(long)this.lastOPosition]] * (1 - this.RateOfLearning) + this.positionScores[this.boardToUniqueEquivalent[futureBoard.GetBoardID()]] * this.RateOfLearning;
                }
                this.lastOPosition = futureBoard.GetBoardID();
            }
            
            if (this.ThinkingSpeed > 0)
            {
                int milliseconds = 150 * this.ThinkingSpeed;
                Thread.Sleep(milliseconds);
            }

            return move.Position;
        }

    }
}
