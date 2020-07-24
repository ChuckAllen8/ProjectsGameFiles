using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEscape
{
    public class FloorDoor : FloorObject
    {
        public FloorPlan Side1
        {
            get; private set;
        }

        public FloorPlan Side2
        {
            get; private set;
        }

        public Point ExitSide1
        { get; private set; }

        public Point ExitSide2
        { get; private set; }

        public FloorDoor(FloorPlan side1, FloorPlan side2, int side1X, int side1Y, int side2X, int side2Y)
        {
            Side1 = side1;
            Side2 = side2;
            side1[side1X, side1Y] = this;
            side2[side2X, side2Y] = this;

            Point side1Exit = new Point();
            Point side2Exit = new Point();

            if (side1X == 0)
            {
                side1Exit.X = 1;
            }
            else if (side1X == side1.Rows - 1)
            {
                side1Exit.X = side1.Rows - 2;
            }
            else
            {
                side1Exit.X = side1X;
            }

            if (side1Y == 0)
            {
                side1Exit.Y = 1;
            }
            else if (side1Y == side1.Columns - 1)
            {
                side1Exit.Y = side1.Columns - 2;
            }
            else
            {
                side1Exit.Y = side1Y;
            }

            if (side2X == 0)
            {
                side2Exit.X = 1;
            }
            else if (side2X == side2.Rows - 1)
            {
                side2Exit.X = side2.Rows - 2;
            }
            else
            {
                side2Exit.X = side2X;
            }

            if (side2Y == 0)
            {
                side2Exit.Y = 1;
            }
            else if (side2Y == side2.Columns - 1)
            {
                side2Exit.Y = side2.Columns - 2;
            }
            else
            {
                side2Exit.Y = side2Y;
            }

            ExitSide1 = side1Exit;
            ExitSide2 = side2Exit;

        }

        public override char Symbol
        { get { return Visible ? '\u0398' : '\u2588'; } set => base.Symbol = value; }

        public override void Activate(object sender, EventArgs e)
        {
            FloorPlan calling = (FloorPlan)sender;
            if(calling == Side1)
            {
                ConsoleEscape.CurrentFloor = Side2;
            }
            else
            {
                ConsoleEscape.CurrentFloor = Side1;
            }
        }
    }
}
