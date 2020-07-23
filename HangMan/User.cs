using System;
using System.Collections.Generic;
using System.Text;

namespace HangMan
{
    class User
    {

        private bool isPhraseGuess;
        private char[] guess;
        private ConsoleManager game;

        public bool IsPhraseGuess
        {
            get { return isPhraseGuess; }
        }

        public char[] Guess
        {
            get { return guess; }
        }

        public User(ConsoleManager screen)
        {
            isPhraseGuess = false;
            guess = new char[0];
            game = screen;
        }

        public bool PlayAgain()
        {
            string answer = game.GetInput();
            if (answer.Trim().ToUpper() == "Y" || answer.Trim().ToUpper() == "YES")
                return true;
            else
            {
                return false;
            }
        }

        public void GetGuess()
        {
            string input = "";
            while (input == "")
            {
                game.SetInputPosition();
                input = game.GetInput().ToUpper();
            }
            if(input.Trim().Length > 1)
            {
                isPhraseGuess = true;
            }
            else
            {
                isPhraseGuess = false;
            }
            guess = input.Trim().ToCharArray();
        }
    }
}
