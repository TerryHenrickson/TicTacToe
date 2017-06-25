using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public class HumanPlayer : Player
    {
        public HumanPlayer()
        {
            this.Mark = Mark.Cross;
        }

        public override void Reset()
        {
            // do nothing?
        }
    }
}
