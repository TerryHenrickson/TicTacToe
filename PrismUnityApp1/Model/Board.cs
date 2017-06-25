using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public struct Board
    {
        public Mark[,] GridAllocations { get; set; }

        public long GetBoardID()
        {
            long id = 0;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    id += (long)Math.Pow(3, new GridPoint(x,y).Position) * (int)GridAllocations[y, x];
                }
            }

            return id;
        }

        public Board(Mark[,] grid)
        {
            this.GridAllocations = grid;
        }

        public Board(long boardId)
        {
            var newGrid = new Mark[3, 3];

            if (boardId == 0)
            {
                this.GridAllocations = newGrid;
                return;
            }

            for (int position = 0; position < 9; position++)
            {
                var gridPoint = new GridPoint(position);
                newGrid[gridPoint.Y, gridPoint.X] = (Mark)(boardId % 3);
                boardId = boardId / 3;
                if(boardId == 0) { break; }
            }

            this.GridAllocations = newGrid;
        }

        public Board AddMarkToLocation(Mark mark, GridPoint gridPoint)
        {
            var newBoardID = this.GetBoardID() + (long)Math.Pow(3, gridPoint.Position) * (int)mark;
            return new Board(newBoardID);
        }

        public Board AddMarkToLocation(Mark mark, int position)
        {
            var newBoardID = this.GetBoardID() + (long)Math.Pow(3, position) * (int)mark;
            return new Board(newBoardID);
        }

        public Mark GetMarkAtLocation(int position)
        {
            var gridPoint = new GridPoint(position);
            return this.GridAllocations[gridPoint.Y, gridPoint.X];
        }

        public Board RotateBy90()
        {
            var newGrid = new Mark[3, 3];

            newGrid[0, 0] = this.GridAllocations[0, 2];
            newGrid[0, 1] = this.GridAllocations[1, 2];
            newGrid[0, 2] = this.GridAllocations[2, 2];
            newGrid[1, 0] = this.GridAllocations[0, 1];
            newGrid[1, 1] = this.GridAllocations[1, 1];
            newGrid[1, 2] = this.GridAllocations[2, 1];
            newGrid[2, 0] = this.GridAllocations[0, 0];
            newGrid[2, 1] = this.GridAllocations[1, 0];
            newGrid[2, 2] = this.GridAllocations[2, 0];

            return new Board(newGrid);
        }

        public Board FlipLR()
        {
            var newGrid = new Mark[3, 3];

            newGrid[0, 0] = this.GridAllocations[0, 2];
            newGrid[0, 1] = this.GridAllocations[0, 1];
            newGrid[0, 2] = this.GridAllocations[0, 0];
            newGrid[1, 0] = this.GridAllocations[1, 2];
            newGrid[1, 1] = this.GridAllocations[1, 1];
            newGrid[1, 2] = this.GridAllocations[1, 0];
            newGrid[2, 0] = this.GridAllocations[2, 2];
            newGrid[2, 1] = this.GridAllocations[2, 1];
            newGrid[2, 2] = this.GridAllocations[2, 0];

            return new Board(newGrid);
        }

        public Board FlipUD()
        {
            var newGrid = new Mark[3, 3];

            newGrid[2, 0] = this.GridAllocations[0, 0];
            newGrid[2, 1] = this.GridAllocations[0, 1];
            newGrid[2, 2] = this.GridAllocations[0, 2];
            newGrid[1, 0] = this.GridAllocations[1, 0];
            newGrid[1, 1] = this.GridAllocations[1, 1];
            newGrid[1, 2] = this.GridAllocations[1, 2];
            newGrid[0, 0] = this.GridAllocations[2, 0];
            newGrid[0, 1] = this.GridAllocations[2, 1];
            newGrid[0, 2] = this.GridAllocations[2, 2];

            return new Board(newGrid);
        }

        public Board FlipDiag1()
        {
            var newGrid = new Mark[3, 3];

            newGrid[2, 2] = this.GridAllocations[0, 0];
            newGrid[1, 2] = this.GridAllocations[0, 1];
            newGrid[0, 2] = this.GridAllocations[0, 2];
            newGrid[2, 1] = this.GridAllocations[1, 0];
            newGrid[1, 1] = this.GridAllocations[1, 1];
            newGrid[0, 1] = this.GridAllocations[1, 2];
            newGrid[2, 0] = this.GridAllocations[2, 0];
            newGrid[1, 0] = this.GridAllocations[2, 1];
            newGrid[0, 0] = this.GridAllocations[2, 2];

            return new Board(newGrid);
        }

        public Board FlipDiag2()
        {
            var newGrid = new Mark[3, 3];

            newGrid[0, 0] = this.GridAllocations[0, 0];
            newGrid[1, 0] = this.GridAllocations[0, 1];
            newGrid[2, 0] = this.GridAllocations[0, 2];
            newGrid[0, 1] = this.GridAllocations[1, 0];
            newGrid[1, 1] = this.GridAllocations[1, 1];
            newGrid[2, 1] = this.GridAllocations[1, 2];
            newGrid[0, 2] = this.GridAllocations[2, 0];
            newGrid[1, 2] = this.GridAllocations[2, 1];
            newGrid[2, 2] = this.GridAllocations[2, 2];

            return new Board(newGrid);
        }


        public IEnumerable<GridPoint> GetEmptyPositions()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if(this.GridAllocations[y,x] == Mark.None)
                    {
                        yield return new GridPoint(x, y);
                    }
                }
            }
        }

        public Mark GetWinnerIfExists()
        {
            if (this.GridAllocations[0, 0] != Mark.None)
            {
                if (this.GridAllocations[0, 0] == this.GridAllocations[0, 1] && this.GridAllocations[0, 1] == this.GridAllocations[0, 2])
                {
                    return this.GridAllocations[0, 0];
                }

                if (this.GridAllocations[0, 0] == this.GridAllocations[1, 1] && this.GridAllocations[1, 1] == this.GridAllocations[2, 2])
                {
                    return this.GridAllocations[0, 0];
                }

                if (this.GridAllocations[0, 0] == this.GridAllocations[1, 0] && this.GridAllocations[1, 0] == this.GridAllocations[2, 0])
                {
                    return this.GridAllocations[0, 0];
                }
            }

            if (this.GridAllocations[0, 1] != Mark.None)
            {
                if (this.GridAllocations[0, 1] == this.GridAllocations[1, 1] && this.GridAllocations[1, 1] == this.GridAllocations[2, 1])
                {
                    return this.GridAllocations[0, 1];
                }
            }

            if (this.GridAllocations[1, 0] != Mark.None)
            {
                if (this.GridAllocations[1, 0] == this.GridAllocations[1, 1] && this.GridAllocations[1, 1] == this.GridAllocations[1, 2])
                {
                    return this.GridAllocations[1, 0];
                }
            }

            if (this.GridAllocations[2, 0] != Mark.None)
            {
                if (this.GridAllocations[2, 0] == this.GridAllocations[2, 1] && this.GridAllocations[2, 1] == this.GridAllocations[2, 2])
                {
                    return this.GridAllocations[2, 0];
                }
            }

            if (this.GridAllocations[0, 2] != Mark.None)
            {
                if (this.GridAllocations[0, 2] == this.GridAllocations[1, 1] && this.GridAllocations[1, 1] == this.GridAllocations[2, 0])
                {
                    return this.GridAllocations[0, 2];
                }

                if (this.GridAllocations[0, 2] == this.GridAllocations[1, 2] && this.GridAllocations[1, 2] == this.GridAllocations[2, 2])
                {
                    return this.GridAllocations[0, 2];
                }
            }

            foreach (var gridPoint in this.GridAllocations)
            {
                if(gridPoint == Mark.None)
                {
                    return Mark.None;
                }
            }

            return (Mark.Cross | Mark.Nought);
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

            yield return this.FlipUD();
            yield return this.FlipUD().RotateBy90();
            yield return this.FlipUD().RotateBy90().RotateBy90(); ;
            yield return this.FlipUD().RotateBy90().RotateBy90().RotateBy90();

            yield return this.FlipDiag1();
            yield return this.FlipDiag1().RotateBy90();
            yield return this.FlipDiag1().RotateBy90().RotateBy90(); ;
            yield return this.FlipDiag1().RotateBy90().RotateBy90().RotateBy90();

            yield return this.FlipDiag2();
            yield return this.FlipDiag2().RotateBy90();
            yield return this.FlipDiag2().RotateBy90().RotateBy90(); ;
            yield return this.FlipDiag2().RotateBy90().RotateBy90().RotateBy90();
        }
    }
}
