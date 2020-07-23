using System;
using System.Collections.Generic;
using System.Text;

namespace RoShamBo
{
    class UserPlayer : Player
    {

        public UserPlayer()
        {
            base.winningPhrase = "I beat the machine!";
            base.choice = "";
        }

        public override void MakeChoice()
        {
            string userChoice = "";
            do
            {
                Console.WriteLine("(R)ock | (P)aper | (S)cissors");
                userChoice = Console.ReadLine().ToUpper().Trim();
                Console.WriteLine(""); //for formatting
            } while (!ValidChoice(userChoice));
            choice = userChoice;
        }

        public bool GoAgain()
        {
            string userChoice;

            Console.WriteLine("Would you like to go again? (Y/Yes, anything else quits)");
            userChoice = Console.ReadLine();
            Console.WriteLine("\n"); //format anything after to come with a space in between.
            if(userChoice.ToUpper() == "Y" || userChoice.ToUpper() == "YES")
            {
                return true;
            }
            return false;
        }

        private bool ValidChoice(string input)
        {
            if(input == "R" || input == "S" || input == "P")
            {
                return true;
            }
            else
            {
                Console.WriteLine("That was invalid, try again.");
                return false;
            }
        }
    }
}
