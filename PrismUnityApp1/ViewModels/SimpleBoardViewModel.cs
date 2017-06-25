using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model;

namespace TicTacToe.ViewModels
{
    public class SimpleBoardViewModel : BindableBase
    {
        private Board board;

        public SimpleBoardViewModel(Board board)
        {
            this.board = board;
        }

        private string MarkToString(Mark mark)
        {
            switch (mark)
            {
                case Mark.None:
                    return string.Empty;
                case Mark.Cross:
                    return "X";
                case Mark.Nought:
                    return "O";
                default:
                    return string.Empty;
            }
        }

        public string TopLeft { get { return this.MarkToString(board.GridAllocations[0, 0]); } }
        public string TopMiddle { get { return this.MarkToString(board.GridAllocations[0, 1]); } }
        public string TopRight { get { return this.MarkToString(board.GridAllocations[0, 2]); } }
        public string MiddleLeft { get { return this.MarkToString(board.GridAllocations[1, 0]); } }
        public string MiddleMiddle { get { return this.MarkToString(board.GridAllocations[1, 1]); } }
        public string MiddleRight { get { return this.MarkToString(board.GridAllocations[1, 2]); } }
        public string BottomLeft { get { return this.MarkToString(board.GridAllocations[2, 0]); } }
        public string BottomMiddle { get { return this.MarkToString(board.GridAllocations[2, 1]); } }
        public string BottomRight { get { return this.MarkToString(board.GridAllocations[2, 2]); } }

        public bool CanPlayInPosition(int position)
        {
            return this.board.GetMarkAtLocation(position) == Mark.None && this.board.GetWinnerIfExists() == Mark.None;
        }

    }
}
