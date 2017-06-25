using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public struct GridPoint
    {
        private int x;

        private int y;

        public GridPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public GridPoint(int position)
        {
            this.x = position % 3;
            this.y = (position - this.x) / 3;
        }

        public int X { get { return this.x; } }

        public int Y { get { return this.y; } }

        public int Position { get { return GetPosition(this); } }

        public static int GetPosition(GridPoint gridPoint)
        {
            return gridPoint.Y * 3 + gridPoint.X;
        }
    }
}
