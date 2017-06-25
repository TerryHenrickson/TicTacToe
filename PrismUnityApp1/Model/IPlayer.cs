using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model.DataObjects;

namespace TicTacToe.Model
{
    public interface IPlayer
    {
        Board Board { get; }
        Game Game { get; set; }
        Mark Mark { get; set; }
        string Name { get; set; }
        ScoreStats ScoreStats { get; set; }
        GameEndType? HasWon { get; }

        event PropertyChangedEventHandler PropertyChanged;

        void Reset();
        bool CanIPlayMark(Mark markToPlay);
        void SetBoardPosition(Board board, Mark previousMark);
        int? GetMove(Mark mark);
    }
}
