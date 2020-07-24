using System;

namespace ConsoleEscape
{
    public class Player : FloorObject
    {
        //base class for all players, user or AI
        public event EventHandler<EventArgs> InputReceived;

        public Player(int x, int y, char symbol)
        {
            X = x;
            Y = y;
            Symbol = symbol;
        }

        public void Listen()
        {
            if (Console.KeyAvailable)
            {
                InputReceived?.Invoke(this, new EventArgs());
            }
        }
    }
}
