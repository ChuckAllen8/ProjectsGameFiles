using System;

namespace ConsoleEscape
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class FloorObject
    {
        //base class for anything that will be in the floor plan
        //every object in this game will be a FloorObject
        //this is where the name, location, and some basic data
        //will be held
        

        public virtual void Activate(object sender, EventArgs e)
        { }

        public bool Visible { get; set; } = true;
        public bool Movable { get; set; } = false;

        public int X { get; set; }
        public int Y { get; set; }
        public Point Location
        {
            get { return new Point(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public char Symbol { get; set; }
    }
}
