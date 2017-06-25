using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    [Flags]
    public enum Mark
    {
        None = 0,
        Cross = 1,
        Nought = 2
    }
}
