using System;
using System.Collections.Generic;
using System.Text;

/*
 * This board is where the game will be displayed.
 * There are different sections for each aspect of the game and it handles
 * updating those sections as things change in the game.
*/


namespace HangMan
{
    class GameBoard
    {
        private struct BoardSection
        {
            public int startX, stopX, startY, stopY;
            public char[,] totalArea;
        }
        private struct ScreenCoordinate
        {
            public int x, y;
        }

        private BoardSection hangingArea, rulesArea, phraseArea, guessesArea, inputArea;
        private ScreenCoordinate inputStart, hangmanStart;
        private char[,] defaultHangingArea, defaultRulesArea, defaultPhraseArea, defaultGuessesArea, defaultInputArea;
        private char[,] phraseToGuess, guessesMade, hangman;

        public GameBoard()
        {

            //The first section is the section for the hangman, we'll make this 10x10
            hangingArea.startX = hangingArea.startY = 0;
            hangingArea.stopX = hangingArea.stopY = 9;
            hangingArea.totalArea = new char[10,10];

            //The next section on top right is for the rules
            rulesArea.startX = 10;
            rulesArea.startY = 0;
            rulesArea.stopX = 19;
            rulesArea.stopY = 3;
            rulesArea.totalArea = new char[4,10];

            //Below the rules is the area for the game outcome.
            guessesArea.startX = 10;
            guessesArea.startY = 4;
            guessesArea.stopX = 19;
            guessesArea.stopY = 9;
            guessesArea.totalArea = new char[6,10];

            //Below all of that should be the phrase to be guessed.
            phraseArea.startX = 0;
            phraseArea.startY = 10;
            phraseArea.stopX = 19;
            phraseArea.stopY = 14;
            phraseArea.totalArea = new char[5,20];

            //At the bottom is the area for input
            inputArea.startX = 0;
            inputArea.startY = 15;
            inputArea.stopX = 19;
            inputArea.stopY = 19;
            inputArea.totalArea = new char[5,20];

            //The place where all input will be taken from
            inputStart.y = 16;
            inputStart.x = 2;

            //where in the hanging area the hangman starts
            hangmanStart.x = 4;
            hangmanStart.y = 3;

            phraseToGuess = new char[3,18];
            guessesMade = new char[4,8];
            hangman = new char[4,3];

            SetDefaults();
            hangingArea.totalArea = (char[,])defaultHangingArea.Clone();
            rulesArea.totalArea = (char[,])defaultRulesArea.Clone();
            phraseArea.totalArea = (char[,])defaultPhraseArea.Clone();
            guessesArea.totalArea = (char[,])defaultGuessesArea.Clone();
            inputArea.totalArea = (char[,])defaultInputArea.Clone();
        }

        public void RefreshBoard()
        {
            Console.CursorVisible = false;
            DisplaySection(hangingArea.startX, hangingArea.startY, hangingArea.totalArea);
            DisplaySection(rulesArea.startX, rulesArea.startY, rulesArea.totalArea);
            DisplaySection(phraseArea.startX, phraseArea.startY, phraseArea.totalArea);
            DisplaySection(guessesArea.startX, guessesArea.startY, guessesArea.totalArea);
            DisplaySection(inputArea.startX, inputArea.startY, inputArea.totalArea);
            SetInputPosition();
        }

        public void ClearBoard()
        {
            Console.Clear();
            Console.CursorVisible = false;
            hangingArea.totalArea = (char[,])defaultHangingArea.Clone();
            rulesArea.totalArea = (char[,])defaultRulesArea.Clone();
            phraseArea.totalArea = (char[,])defaultPhraseArea.Clone();
            guessesArea.totalArea = (char[,])defaultGuessesArea.Clone();
            inputArea.totalArea = (char[,])defaultInputArea.Clone();
            RefreshBoard();
        }

        private void DisplaySection(int startX, int startY, char[,] section)
        {
            for (int i = 0; i < section.GetLength(0); i++)
            {
                for (int j = 0; j < section.GetLength(1); j++)
                {
                    Console.SetCursorPosition(j + startX, i + startY);
                    Console.Write(section[i, j]);
                }
            }
        }

        public char[] GetBoardInput()
        {
            SetInputPosition();
            List<char> input = new List<char>();
            ConsoleKey keyRead = Console.ReadKey().Key;

            while(keyRead != ConsoleKey.Enter)
            {
                input.Add(keyRead.ToString().ToCharArray()[0]);
                keyRead = Console.ReadKey().Key;
                if(Console.CursorLeft > 18)
                {
                    Console.SetCursorPosition(inputStart.x, Console.CursorTop + 1);
                }
            }
            ClearInputArea();
            SetInputPosition();
            return input.ToArray();
        }

        private void ClearInputArea()
        {
            inputArea.totalArea = (char[,])defaultInputArea.Clone();
            RefreshBoard();
        }

        private void SetInputPosition()
        {
            Console.SetCursorPosition(inputStart.x, inputStart.y);
            Console.CursorVisible = true;
        }

        public void UpdateHangingArea(char[,] updatedHangman)
        {
            hangman = updatedHangman;
            for(int i = 0; i < hangman.GetLength(0); i++)
            {
                for (int j = 0; j < hangman.GetLength(1); j++)
                {
                    hangingArea.totalArea[i + hangmanStart.x, j + hangmanStart.y] = hangman[i, j];
                }
            }
            RefreshBoard();
        }
        public void UpdateGuessesArea(char[,] updatedGuesses)
        {
            guessesMade = updatedGuesses;
            for (int i = 0; i < guessesMade.GetLength(0); i++)
            {
                for (int j = 0; j < guessesMade.GetLength(1); j++)
                {
                    guessesArea.totalArea[i + 1, j + 1] = guessesMade[i, j];
                }
            }
            RefreshBoard();
        }

        public void UpdatePhrasesArea(char[,] updatedPhrase)
        {
            phraseToGuess = updatedPhrase;
            for (int i = 0; i < phraseToGuess.GetLength(0); i++)
            {
                for (int j = 0; j < phraseToGuess.GetLength(1); j++)
                {
                    phraseArea.totalArea[i + 1, j + 1] = phraseToGuess[i, j];
                }
            }
            RefreshBoard();
        }

        private void SetDefaults()
        {
            //defaultHangingArea, defaultRulesArea, defaultPhraseArea, defaultGuessesArea, defaultInputArea, defaultOutcomeArea;
            defaultHangingArea = new char[10, 10]
            {
                { '~', '~', '~', '~', '~', '~', '~', '~', '~', '~' },
                { '|', ' ', '/', '-', '-', '-', ' ', ' ', ' ', '|' },
                { '|', ' ', '|', ' ', ' ', '|', ' ', ' ', ' ', '|' },
                { '|', ' ', '|', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', '|', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', '|', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', '|', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', '|', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', 'L', '_', '_', '_', ' ', ' ', ' ', '|' },
                { '\\', '_', '_', '_', '_', '_', '_', '_', '_', '/' }
            };

            defaultRulesArea = new char[4, 10]
            {
                { '~', '~', '~', '~', '~', '~', '~', '~', '~', '~' },
                { '|', 'G', 'u', 'e', 's', 's', ' ', 'a', ' ', '|' },
                { '|', 'l', 'e', 't', 't', 'e', 'r', ' ', 'o', '|' },
                { '|', 'r', ' ', 'p', 'h', 'r', 'a', 's', 'e', '|' }
            };
            defaultPhraseArea = new char[5, 20]
            {
                
                { '|', '~', '~', '~', '~', '~', '~', '~', '~', '~', '~', '~', '~', '~', '~', '~', '~', '~', '~', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '|' }
            };

            defaultGuessesArea = new char[6, 10]
            {
                { '|', '-', '-', '-', '-', '-', '-', '-', '-', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '\\', '_', '_', '_', '_', '_', '_', '_', '_', '/' }
            };
            defaultInputArea = new char[5, 20]
            {
                { '|', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|' },
                { '\\', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '/' }
            };
        }
    }
}
