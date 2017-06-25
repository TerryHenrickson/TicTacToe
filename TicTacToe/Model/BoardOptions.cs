using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public class BoardOptions
    {
        private Dictionary<long, long> boardToUniqueEquivalent = new Dictionary<long, long>();

        public BoardOptions()
        {
            var boards = new List<Board>();

            this.CreateBoardOptions(boards, new Board(0), Mark.Cross);
            var uniqueBoardIDs = boards.Select(board => board.GetBoardID()).Distinct().ToList();

            while (uniqueBoardIDs.Any())
            {
                var uniqueBoardID = uniqueBoardIDs.First();
                var similarBoards = new List<long> { uniqueBoardID };

                similarBoards.AddRange(new Board(uniqueBoardID).GetquivalentBoardIds().Select(b => b.GetBoardID()));

                foreach (var boardID in similarBoards)
                {
                    uniqueBoardIDs.Remove(boardID);
                    this.boardToUniqueEquivalent.Add(uniqueBoardID, boardID);
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
