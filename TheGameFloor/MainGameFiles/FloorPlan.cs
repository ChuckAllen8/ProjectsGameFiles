using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConsoleEscape
{
    public delegate void ObjectMovedDelegate(object sender, EventArgs args);


    public class FloorPlan : FloorObject
    {
        //this will be the map of where everything is on the game floor.

        private FloorObject[,] floorPlan;
        public event ObjectMovedDelegate ObjectMoved;
        public List<Player> miniGamePlayers;

        public FloorPlan(int x, int y)
        {
            floorPlan = new FloorObject[25, 50];
            miniGamePlayers = new List<Player>();
            Symbol = '\u0398';
            X = x;
            Y = y;
        }

        public FloorObject this[int row, int column]
        {
            get { return floorPlan[row, column]; }
            set
            {
                try
                {
                    floorPlan[row, column] = value;
                    ObjectMoved?.Invoke(this, new EventArgs());
                }
                catch
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public bool IsOccupied(int x, int y)
        {
            try
            {
                return (floorPlan[x, y] != null);
            }
            catch
            {
                return true;
            }
        }


        public override void Activate(object sender, EventArgs e)
        {
            Console.Clear();
            ConsoleEscape.CurrentFloor = this;
        }

        public void Draw()
        {
            StringBuilder theLine = new StringBuilder();
            for (int row = 0; row < Rows; row++)
            {
                if(row != 0)
                {
                    Console.WriteLine();
                }
                Console.SetCursorPosition(0, row + 1);
                for (int column = 0; column < Columns; column++)
                {
                    char toDraw = ' ';
                    if (this[row, column] != null && this[row, column].Visible)
                    {
                        toDraw = this[row, column].Symbol;
                    }
                    theLine.Append(toDraw);
                }
                Console.Write(theLine);
                theLine.Clear();
            }
        }

        public void AddPiece(FloorObject piece)
        {
            this[piece.X, piece.Y] = piece;
        }

        public void RemovePiece(FloorObject piece)
        {
            this[piece.X, piece.Y] = null;
        }

        public int Rows
        { get { return floorPlan.GetLength(0); } }

        public int Columns
        { get { return floorPlan.GetLength(1); } }

        public void UserActivated(Player piece, int checkX, int checkY)
        {
            if (checkX >= 0 && checkX < Rows && checkY >= 0 && checkY < Columns && this.IsOccupied(checkX, checkY) && this[checkX, checkY] != null)
            {
                if (this[checkX, checkY].GetType() == typeof(FloorPlan))
                {
                    RemovePiece(piece);
                    if(X + 1 >= Rows)
                    {
                        piece.X = X - 1;
                    }
                    else
                    {
                        piece.X = X + 1;
                    }
                    if(Y + 1 >= Columns)
                    {
                        piece.Y = Y - 1;
                    }
                    else
                    {
                        piece.Y = Y + 1;
                    }
                    ((FloorPlan)this[checkX, checkY]).AddPiece(piece);
                }
                this[checkX, checkY].Activate(this, new EventArgs());
            }
        }

        
    }
}