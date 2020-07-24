using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEscape.MainGameFiles
{
    class Controller
    {
        public void UserInput(object user, EventArgs e)
        {

            Player piece = (Player)user;
            List<ConsoleKey> inputs = new List<ConsoleKey>();
            FloorPlan floor = ConsoleEscape.CurrentFloor;

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
            Point newLocation = new Point(piece.X, piece.Y);
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

            //check collision and within bounds before moving, and if a collision occurs but the object is movable then move it (unless it would collide or move out of bounds)
            if (!floor.IsOccupied(newLocation.X, newLocation.Y))
            {
                floor[piece.X, piece.Y] = null;
                floor[newLocation.X, newLocation.Y] = piece;
                piece.Location = newLocation;
            }
            else if (newLocation.X >= 0 && newLocation.Y >= 0 && newLocation.X < floor.Rows && newLocation.Y < floor.Columns && floor[newLocation.X, newLocation.Y] != null && floor[newLocation.X, newLocation.Y].Movable)
            {
                FloorObject movablePiece = floor[newLocation.X, newLocation.Y];
                int tempX = movablePiece.X + (newLocation.X - piece.X);
                int tempY = movablePiece.Y + (newLocation.Y - piece.Y);
                if (tempX < floor.Rows && tempY < floor.Columns && !floor.IsOccupied(tempX, tempY))
                {
                    floor[piece.X, piece.Y] = null;
                    floor[newLocation.X, newLocation.Y] = piece;
                    floor[tempX, tempY] = movablePiece;
                    movablePiece.Location = new Point(tempX, tempY);
                    piece.Location = newLocation;
                }
            }

            //user selected activate, check the four squares and activate the objects in them.
            if (inputs.Contains(ConsoleKey.Enter))
            {
                int tempX = piece.X;
                int tempY = piece.Y;
                floor.UserActivated(piece, tempX + 1, tempY);
                floor.UserActivated(piece, tempX - 1, tempY);
                floor.UserActivated(piece, tempX, tempY + 1);
                floor.UserActivated(piece, tempX, tempY - 1);
            }
        }
    }
}
