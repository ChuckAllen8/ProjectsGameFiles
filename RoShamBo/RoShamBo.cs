using System;
using System.Collections.Generic;
using System.Text;

namespace RoShamBo
{
    public class RoShamBo
    {
        private ComputerPlayer computer;
        private UserPlayer user;

        public static void Run(object sender, EventArgs e)
        {
            RoShamBo game = new RoShamBo();
            game.Start();
            Console.Clear();
        }

        public void Start()
        {
            bool gaming = true;

            computer = new ComputerPlayer();
            user = new UserPlayer();
            Console.SetWindowSize(120, 25);

            Console.WriteLine("The game is RoShamBo, also called Rock, Paper, Scissors");
            Console.WriteLine("You must choose between the three options, with Rock beating Scissors, Scissors beats Paper, and Paper beats Rock.");
            Console.WriteLine("If you and your opponent pick the same thing you must go again to break the tie.\n");

            while (gaming)
            {
                string winningChoice = "T";

                while (winningChoice == "T")
                {
                    computer.MakeChoice();
                    user.MakeChoice();

                    winningChoice = WhoWins(user.Choice, computer.Choice);
                    if (winningChoice == "T")
                    {
                        Console.WriteLine($"That was a tie, you both chose {user.Choice}, go again!");
                    }
                }

                DisplayWinner(winningChoice);
                gaming = user.GoAgain();
            }
        }

        private string WhoWins(string playerOne, string playerTwo)
        {
            if(playerOne == playerTwo)
            {
                return "T";
            }
            else if(playerOne == "R" && playerTwo == "S")
            {
                return playerOne;
            }
            else if(playerOne == "P" && playerTwo == "R")
            {
                return playerOne;
            }
            else if(playerOne == "S" && playerTwo == "P")
            {
                return playerOne;
            }
            return playerTwo;
        }

        private void DisplayWinner(string winningChoice)
        {
            string winner;
            if(winningChoice == user.Choice)
            {
                winner = "the player";
            }
            else
            {
                winner = "the computer";
            }

            Console.WriteLine($"The choices were {user.Choice} and {computer.Choice}");
            Console.WriteLine($"This means that {winner} won!");

            if(winner == "the player")
            {
                user.Winner();
            }
            else
            {
                computer.Winner();
            }
        }
    }
}
