using ConsoleEscape.MainGameFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleEscape
{

    public static class ConsoleEscape
    {
        public const int TICKS_PER_ROUND = 250000;
        //create a game floor, fill it with games and players, and allow for
        //the user to walk around and select different games to play

        /* 
         * 
         * If I move the current floor info to the User and give it an
         * input and output stream, I could iterate over all users
         * and have each one on a separate map, that can then be drawn
         * to their screen.
         * 
         * I can also store a dictionary of maps, with <mapID,mapObj>
         * here that when the user switches map or gets added it all
         * uses the same object so two players could be on the same map
         * at the same time.
         * 
         * 
         */

        
        public static FloorPlan CurrentFloor
        { get; set; }

        public static void Run()
        {
            Console.Clear();
            Console.SetWindowSize(50, 25);
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Title = "Console Escape!";

            FloorPlan floorPlan = new FloorPlan(1, 10);
            FloorPlan room2 = new FloorPlan(13, 49);

            Controller controller1 = new Controller();

            Player user = new Player(1, 1, 'C');
            user.InputReceived += controller1.UserInput;

            DateTime tickStart = DateTime.Now;
            DateTime now = DateTime.Now;

            SetMap(floorPlan, Environment.CurrentDirectory + @"\MapFiles\MainMap\");
            SetMap(room2, Environment.CurrentDirectory + @"\MapFiles\SideMap\");

            
            //testing doors, added a door on each side to go to the opposite side of the other room
            FloorDoor firstDoor = new FloorDoor(floorPlan, room2, 0, 10, room2.Rows - 1, 10);
            FloorDoor secondDoor = new FloorDoor(floorPlan, room2, floorPlan.Rows-1, 10, 0, 10);
            FloorDoor thirdDoor = new FloorDoor(floorPlan, room2, 10, floorPlan.Columns - 1, 10, 0);
            FloorDoor fourthDoor = new FloorDoor(floorPlan, room2, 10, 0, 10, room2.Columns - 1);

            floorPlan.WallIn();
            room2.WallIn();

            firstDoor.Visible = false;
            secondDoor.Visible = false;

            floorPlan.AddPiece(user);

            CurrentFloor = floorPlan;

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
                //Update?.Invoke();
                CurrentFloor.Draw();
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
                thisPlayer.Movable = true;
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
