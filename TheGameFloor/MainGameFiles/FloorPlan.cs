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

        public event UpdateConsoleDelegate DisplayRoom;

        public override void Activate(object sender, EventArgs e)
        {
            if (DisplayRoom != null)
            {
                Console.Clear();
                FloorPlan game = (FloorPlan) sender;
                ConsoleEscape.Update -= game.DisplayRoom;
                ConsoleEscape.Update += this.DisplayRoom;
            }
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
                    //this was the old draw logic
                    //Console.SetCursorPosition(column + 1, row + 1);
                    //Console.Write(toDraw);
                    /* 
                     * It has been replaced due to being slow, the new logic creates each
                     * line and then draws the line rather than drawing each character
                     * 1 by 1. This results in smoother input and clearer displaying
                     * 
                     */
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
                    piece.InputReceived -= UserInput;
                    piece.InputReceived += ((FloorPlan)this[checkX, checkY]).UserInput;
                    ((FloorPlan)this[checkX, checkY]).AddPiece(piece);
                }
                this[checkX, checkY].Activate(this, new EventArgs());
            }
        }

        public void UserInput(object user, EventArgs e)
        {

            Player piece = (Player)user;
            List<ConsoleKey> inputs = new List<ConsoleKey>();

            while (Console.KeyAvailable)
            {
                ConsoleKey currKey = Console.ReadKey().Key;
                if (inputs.Contains(currKey))
                {
                    break;
                }
                inputs.Add(currKey);
            }

            //check left or right movement
            FloorObject.Point newLocation = new FloorObject.Point(piece.X, piece.Y);
            if (inputs.Contains(ConsoleKey.LeftArrow))
            {
                newLocation.Y--;
            }
            else if (inputs.Contains(ConsoleKey.RightArrow))
            {
                newLocation.Y++;
            }
            //check up or down movement
            if (inputs.Contains(ConsoleKey.UpArrow))
            {
                newLocation.X--;
            }
            else if (inputs.Contains(ConsoleKey.DownArrow))
            {
                newLocation.X++;
            }
            if (!this.IsOccupied(newLocation.X, newLocation.Y))
            {
                this[piece.X, piece.Y] = null;
                this[newLocation.X, newLocation.Y] = piece;
                piece.Location = newLocation;
            }
            else if (this[newLocation.X, newLocation.Y] != null && this[newLocation.X, newLocation.Y].Movable)
            {
                FloorObject movablePiece = (FloorObject) this[newLocation.X, newLocation.Y];
                int tempX = movablePiece.X + (newLocation.X - piece.X);
                int tempY = movablePiece.Y + (newLocation.Y - piece.Y);
                if(tempX < floorPlan.GetLength(0) && tempY < floorPlan.GetLength(1) && !this.IsOccupied(tempX, tempY))
                {
                    this[piece.X, piece.Y] = null;
                    this[newLocation.X, newLocation.Y] = piece;
                    this[tempX, tempY] = movablePiece;
                    movablePiece.Location = new Point(tempX, tempY);
                    piece.Location = newLocation;
                }
            }

            if (inputs.Contains(ConsoleKey.Enter))
            {
                int tempX = piece.X;
                int tempY = piece.Y;
                UserActivated(piece, tempX + 1, tempY);
                UserActivated(piece, tempX - 1, tempY);
                UserActivated(piece, tempX, tempY + 1);
                UserActivated(piece, tempX, tempY - 1);
            }
        }
    }
}