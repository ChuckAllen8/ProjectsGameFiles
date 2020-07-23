using System;
using System.Collections.Generic;
using System.Text;

namespace HangMan
{
    public class HangMan
    {
        private bool isAlive;
        private int wrongGuesses;
        private char[,] display;

        public static void Run(object sender, EventArgs e)
        {
            HangingGame game = new HangingGame();
            game.Start();
            Console.Clear();
        }

        public HangMan()
        {
            isAlive = true;
            wrongGuesses = 0;
            display = DefaultHangMan();

        }

        public bool IsAlive
        {
            get { return isAlive; }
        }

        public char[,] GetHangMan()
        {
            return display;
        }

        private char[,] DefaultHangMan()
        {
            char[,] baseHangman = new char[7, 7];

            //top bar
            baseHangman[0, 0] = '_';
            baseHangman[0, 1] = '_';
            baseHangman[0, 2] = '_';
            baseHangman[0, 3] = '_';
            //main post
            baseHangman[1, 0] = '|';
            baseHangman[2, 0] = '|';
            baseHangman[3, 0] = '|';
            baseHangman[4, 0] = '|';
            baseHangman[5, 0] = '|';
            //base bar
            baseHangman[6, 0] = 'L';
            baseHangman[6, 1] = '_';
            baseHangman[6, 2] = '_';
            baseHangman[6, 3] = '_';
            baseHangman[6, 4] = '_';
            //the noose
            baseHangman[1, 3] = '|';

            return baseHangman;
        }

        public void ResetHangMan()
        {
            isAlive = true;
            wrongGuesses = 0;
            display = DefaultHangMan();
        }
        public void AddWrongGuess()
        {
            wrongGuesses++;
            switch (wrongGuesses)
            {
                case 1:
                    display[2, 3] = 'O';
                    break;
                case 2:
                    display[3, 3] = '|';
                    break;
                case 3:
                    display[3, 2] = '-';
                    break;
                case 4:
                    display[3, 4] = '-';
                    break;
                case 5:
                    display[4, 3] = '^';
                    break;
                case 6:
                    display[5, 2] = '/';
                    break;
                default:
                    display[5, 4] = '\\';
                    isAlive = false;
                    break;
            }
        }
    }
}
