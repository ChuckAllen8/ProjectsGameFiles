using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleEscape
{
    public delegate void UpdateConsoleDelegate();
    public static class TheGameFloor
    {
        public const int TICKS_PER_ROUND = 250000;
        //create a game floor, fill it with games and players, and allow for
        //the user to walk around and select different games to play

        public static event UpdateConsoleDelegate Update;

        public static void Run()
        {
            Console.Clear();
            Console.SetWindowSize(50, 25);
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
            Console.Title = "This is a game test!";
            FloorPlan floorPlan = new FloorPlan(1, 10);
            FloorPlan room2 = new FloorPlan(13, 49);
            DateTime tickStart = DateTime.Now;
            DateTime now = DateTime.Now;

            SetMap(floorPlan, Environment.CurrentDirectory + @"\MapFiles\MainMap\");
            SetMap(room2, Environment.CurrentDirectory + @"\MapFiles\SideMap\");

            room2.DisplayRoom += room2.Draw;
            floorPlan.DisplayRoom += floorPlan.Draw;
            Update += floorPlan.Draw;

            room2.AddPiece(floorPlan);
            floorPlan.AddPiece(room2);

            Player user = new Player(1, 1, 'C');
            user.InputReceived += floorPlan.UserInput;

            floorPlan.AddPiece(user);

            while (user.X < Console.WindowHeight)
            {
                Console.SetWindowSize(50, 25);
                Console.CursorVisible = false;
                now = DateTime.Now;
                if ((now - tickStart).Ticks >= TICKS_PER_ROUND)
                {
                    tickStart = DateTime.Now;
                    user.Listen();
                }
                Update?.Invoke();
            }
        }

        public static void SetMap(FloorPlan addTo, string filePath)
        {
            StreamReader reader = new StreamReader(filePath + "FiveCardDrawGames.txt");
            while (!reader.EndOfStream)
            {
                string[] args = reader.ReadLine().Split(",");
                Game thisGame = new Game(int.Parse(args[1]), int.Parse(args[2]), char.Parse(args[0]));
                thisGame.RunGame += FiveCardDraw.FiveCardDraw.Run;
                addTo.AddPiece(thisGame);
            }

            reader.Close();
            reader = new StreamReader(filePath + "HangManGames.txt");

            while (!reader.EndOfStream)
            {
                string[] args = reader.ReadLine().Split(",");
                Game thisGame = new Game(int.Parse(args[1]), int.Parse(args[2]), char.Parse(args[0]));
                thisGame.RunGame += HangMan.HangMan.Run;
                addTo.AddPiece(thisGame);
            }

            reader.Close();
            reader = new StreamReader(filePath + "NPCs.txt");
            
            while (!reader.EndOfStream)
            {
                string[] args = reader.ReadLine().Split(",");
                Player thisPlayer = new Player(int.Parse(args[1]), int.Parse(args[2]), char.Parse(args[0]));
                //thisPlayer.InputReceived += AI.AI.Think;
                addTo.AddPiece(thisPlayer);
            }
            reader.Close();
            reader = new StreamReader(filePath + "RoShamBoGames.txt");

            while (!reader.EndOfStream)
            {
                string[] args = reader.ReadLine().Split(",");
                Game thisGame = new Game(int.Parse(args[1]), int.Parse(args[2]), char.Parse(args[0]));
                thisGame.RunGame += RoShamBo.RoShamBo.Run;
                addTo.AddPiece(thisGame);
            }

            reader.Close();
        }
    }
}
