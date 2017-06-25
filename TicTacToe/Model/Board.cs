using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public struct Board
    {
        private Mark[,] gridAllocations;

        public long GetBoardID()
        {
            long id = 0;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    id += (long)Math.Pow(3, new GridPoint(x,y).Position) * (int)gridAllocations[y, x];
                }
            }

            return id;
        }

        public Board(Mark[,] grid)
        {
            this.gridAllocations = grid;
        }

        public Board(long boardId)
        {
            var newGrid = new Mark[2, 2];

            if (boardId == 0)
            {
                this.gridAllocations = newGrid;
            }

            for (int position = 0; position < 9; position++)
            {
                var gridPoint = new GridPoint(position);
                newGrid[gridPoint.Y, gridPoint.X] = (Mark)(boardId % 3);
                boardId = boardId / 3;
                if(boardId == 0) { break; }
            }

            this.gridAllocations = newGrid;
        }

        public Board AddMarkToLocation(Mark mark, GridPoint gridPoint)
        {
            var newBoardID = this.GetBoardID() + (long)Math.Pow(3, gridPoint.Position) * (int)gridAllocations[gridPoint.Y, gridPoint.X];
            return new Board(newBoardID);
        }
        
        public Board RotateBy90()
        {
            var newGrid = new Mark[2, 2];

            newGrid[0, 0] = this.gridAllocations[0, 2];
            newGrid[0, 1] = this.gridAllocations[1, 2];
            newGrid[0, 2] = this.gridAllocations[2, 2];
            newGrid[1, 0] = this.gridAllocations[0, 1];
            newGrid[1, 1] = this.gridAllocations[1, 1];
            newGrid[1, 2] = this.gridAllocations[2, 1];
            newGrid[2, 0] = this.gridAllocations[0, 0];
            newGrid[2, 1] = this.gridAllocations[1, 0];
            newGrid[2, 2] = this.gridAllocations[2, 0];

            return new Board(newGrid);
        }

        public Board FlipLR()
        {
            var newGrid = new Mark[2, 2];

            newGrid[0, 0] = this.gridAllocations[0, 2];
            newGrid[0, 1] = this.gridAllocations[0, 1];
            newGrid[0, 2] = this.gridAllocations[0, 0];
            newGrid[1, 0] = this.gridAllocations[1, 2];
            newGrid[1, 1] = this.gridAllocations[1, 1];
            newGrid[1, 2] = this.gridAllocations[1, 0];
            newGrid[2, 0] = this.gridAllocations[2, 2];
            newGrid[2, 1] = this.gridAllocations[2, 1];
            newGrid[2, 2] = this.gridAllocations[2, 0];

            return new Board(newGrid);
        }

        public IEnumerable<GridPoint> GetEmptyPositions()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if(this.gridAllocations[y,x] == Mark.None)
                    {
                        yield return new GridPoint(x, y);
                    }
                }
            }
        }

        public Mark GetWinnerIfExists()
        {
            if (this.gridAllocations[0, 0] != Mark.None)
            {
                if (this.gridAllocations[0, 0] == this.gridAllocations[0, 1] && this.gridAllocations[0, 1] == this.gridAllocations[0, 2])
                {
                    return this.gridAllocations[0, 0];
                }

                if (this.gridAllocations[0, 0] == this.gridAllocations[1, 1] && this.gridAllocations[1, 1] == this.gridAllocations[2, 2])
                {
                    return this.gridAllocations[0, 0];
                }

                if (this.gridAllocations[0, 0] == this.gridAllocations[1, 0] && this.gridAllocations[1, 0] == this.gridAllocations[2, 0])
                {
                    return this.gridAllocations[0, 0];
                }
            }

            if (this.gridAllocations[0, 1] != Mark.None)
            {
                if (this.gridAllocations[0, 1] == this.gridAllocations[1, 1] && this.gridAllocations[1, 1] == this.gridAllocations[2, 1])
                {
                    return this.gridAllocations[0, 1];
                }
            }

            if (this.gridAllocations[1, 0] != Mark.None)
            {
                if (this.gridAllocations[1, 0] == this.gridAllocations[1, 1] && this.gridAllocations[1, 1] == this.gridAllocations[1, 2])
                {
                    return this.gridAllocations[1, 0];
                }
            }

            if (this.gridAllocations[2, 0] != Mark.None)
            {
                if (this.gridAllocations[2, 0] == this.gridAllocations[2, 1] && this.gridAllocations[2, 1] == this.gridAllocations[2, 2])
                {
                    return this.gridAllocations[2, 0];
                }
            }

            if (this.gridAllocations[0, 2] != Mark.None)
            {
                if (this.gridAllocations[0, 2] == this.gridAllocations[1, 1] && this.gridAllocations[1, 1] == this.gridAllocations[2, 0])
                {
                    return this.gridAllocations[0, 2];
                }

                if (this.gridAllocations[0, 2] == this.gridAllocations[1, 2] && this.gridAllocations[1, 2] == this.gridAllocations[2, 2])
                {
                    return this.gridAllocations[0, 2];
                }
            }

            return Mark.None;
        }

        public IEnumerable<Board> GetquivalentBoardIds()
        {
            yield return this.RotateBy90();
            yield return this.RotateBy90().RotateBy90();
            yield return this.RotateBy90().RotateBy90().RotateBy90();
            yield return this.FlipLR();
            yield return this.FlipLR().RotateBy90();
            yield return this.FlipLR().RotateBy90().RotateBy90(); ;
            yield return this.FlipLR().RotateBy90().RotateBy90().RotateBy90();
        }
    }
}
