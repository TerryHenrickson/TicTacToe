using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model.DataObjects
{
    public class ScoreStats
    {
        public long Wins { get; set; }
        public long Draws { get; set; }
        public long Losses { get; set; }
    }
}
