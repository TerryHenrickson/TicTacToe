using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public class ComputerIntelligence
    {
        private Dictionary<long, double> positionScores;
        public Dictionary<long, long> BoardToUniqueEquivalent { get; private set; }



        public Dictionary<long, double> GetPositionScores()
        {
            return new Dictionary<long, double>(this.positionScores);
        }

        public ComputerIntelligence()
        {
            this.CreateBoardToUniqueEquivalentDictionary();
            this.CreateMoveScores();
        }



        private void CreateMoveScores()
        {
            this.positionScores = new Dictionary<long, double>();
            var allPositions = this.BoardToUniqueEquivalent.Values.Distinct();

            foreach (var position in allPositions)
            {
                var winner = new Board(position).GetWinnerIfExists();
                if(winner == Mark.None)
                {
                    this.positionScores.Add(position, 0.5d);
                }
                else
                {
                    this.positionScores.Add(position, 1.0d);
                }
            }
        }

        public void CreateBoardToUniqueEquivalentDictionary()
        {
            this.BoardToUniqueEquivalent = new Dictionary<long, long>();
            var boards = new List<Board>();

            this.CreateBoardOptions(boards, new Board(0), Mark.Cross);
            var uniqueBoardIDs = boards.Select(board => board.GetBoardID()).Distinct().ToList();

            while (uniqueBoardIDs.Any())
            {
                var uniqueBoardID = uniqueBoardIDs.First();
                var similarBoards = new List<long> { uniqueBoardID };

                similarBoards.AddRange(new Board(uniqueBoardID).GetquivalentBoardIds().Select(b => b.GetBoardID()));

                foreach (var boardID in similarBoards.Distinct())
                {
                    this.BoardToUniqueEquivalent.Add(boardID, uniqueBoardID);
                    uniqueBoardIDs.Remove(boardID);     
                }
            }
        }

        public void CreateBoardOptions(List<Board> boards, Board board, Mark mark)
        {
            boards.Add(board);

            if(board.GetWinnerIfExists() != Mark.None)
            {
                return;
            }

            var emptyPositions = board.GetEmptyPositions();

            if (!emptyPositions.Any())
            {
                return;
            }

            foreach (var emptyPosition in emptyPositions)
            {
                this.CreateBoardOptions(boards, board.AddMarkToLocation(mark, emptyPosition), mark == Mark.Cross ? Mark.Nought : Mark.Cross);
            }
            
            return;
        }


    }
}
