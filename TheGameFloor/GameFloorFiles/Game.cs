using System;

namespace TheGameFloor
{
    public delegate void GameRunDelegate(object sender, EventArgs e);

    class Game : FloorObject
    {
        //this will be a reference to a game, that is likely in another project
        //to be run when the user selects the use key while next to one of these
        public event GameRunDelegate RunGame;

        public Game(int x, int y, char symbol)
        {
            X = x;
            Y = y;
            Symbol = symbol;
        }

        public override void Activate(object sender, EventArgs e)
        {
            if (RunGame != null)
            {
                Console.Clear();
                RunGame(sender, e);
            }
        }
    }
}
