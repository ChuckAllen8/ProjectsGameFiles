using System;

namespace TheGameFloor
{
    public delegate void InputReceivedDelegate(object sender, EventArgs args);
    public class Player : FloorObject
    {
        //base class for all players, user or AI
        public event InputReceivedDelegate InputReceived;

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
