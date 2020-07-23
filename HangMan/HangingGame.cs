using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace HangMan
{
    class HangingGame
    {
        private bool playing, gameWon, gameLost;
        private int wrongGuesses;
        private char[,] guesses, phrase, hangman, goAgain, gameOutcomeWin, gameOutcomeLose;
        private string completePhrase;
        private List<char> guessList;
        private GameBoard board;

        public HangingGame()
        {
            board = new GameBoard();
            guessList = new List<char>();

            //to do: fix the phrase making logic
            completePhrase = "This is a guessing game".ToUpper();

            phrase = new char[3, 18];
            guesses = new char[4, 8];
            hangman = new char[4, 3];
            goAgain = new char[4, 8]
            {
                { 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' },
                { 'X', 'P', 'l', 'a', 'y', ' ', ' ', 'X' },
                { 'X', 'A', 'g', 'a', 'i', 'n', '?', 'X' },
                { 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' }
            };
            gameOutcomeWin = new char[3, 18]
            {{ 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' },
                { 'X', ' ', ' ', ' ', 'Y', 'O', 'U', ' ', 'W', 'I', 'N', '!', ' ', ':', ')', ' ', ' ', 'X' },
                { 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' }
            };
            gameOutcomeLose = new char[3, 18]
            {
                { 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' },
                { 'X', ' ', ' ', ' ', 'Y', 'O', 'U', ' ', 'L', 'O', 'S', 'E', ' ', ':', '(', ' ', ' ', 'X' },
                { 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' }
            };
        }

        public void Start()
        {
            playing = true;
            gameWon = gameLost = false;
            board.ClearBoard();
            UpdatePhrase();
            board.RefreshBoard();
            while(playing)
            {
                CheckGuess(board.GetBoardInput());
                if(gameLost || gameWon)
                {
                    playing = PlayAgain();
                    board.ClearBoard();

                }
            }
        }

        private void CheckGuess(char[] userGuess)
        {
            if(userGuess.Length == 1 && !guessList.Contains(userGuess[0]))
            {
                guessList.Add(userGuess[0]);
                UpdateGuesses();
                UpdatePhrase();
            }
            else if (userGuess.Length == 1)
            {
                GuessedAlready();
                return;
            }
            if (userGuess.Length == 1 && !completePhrase.Contains(userGuess[0]))
            {
                wrongGuesses++;
                UpdateHangman();
            }
            else if (userGuess.Length != 1)
            {
            }
            char[,] completeGuess = PhraseHide(PhraseParse(completePhrase, 3, 18), 3, 18);
            char[,] phraseArray = PhraseParse(completePhrase, 3, 18);
            bool allCorrect = true;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 18; j++)
                {
                    if(phraseArray[i, j] != completeGuess[i, j])
                    {
                        allCorrect = false;
                    }
                }
            }
            if(allCorrect)
            {
                gameWon = true;
            }
            else if (wrongGuesses >= 6)
            {
                gameLost = true;
            }
        }

        private void UpdateHangman()
        {

        }

        private void UpdateGuesses()
        {
            board.UpdateGuessesArea(PhraseParse(guessList.ToArray(), 4, 8));
        }

        private void GuessedAlready()
        {
            board.UpdateGuessesArea(PhraseParse("Already guessed that!", 4, 8));
        }

        private void UpdatePhrase()
        {
            board.UpdatePhrasesArea(PhraseHide(PhraseParse(completePhrase, 3, 18), 3, 18));
        }

        private char[,] PhraseHide(char[,] phraseAsArray, int rows, int columns)
        {
            char[,] outputArray = new char[rows, columns];
            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if(guessList.Contains(phraseAsArray[i,j]))
                    {
                        outputArray[i, j] = phraseAsArray[i, j];
                    }
                    else if(phraseAsArray[i, j] != ' ')
                    {
                        outputArray[i, j] = '_';
                    }
                    if(outputArray[i, j] == 0)
                    {
                        outputArray[i, j] = ' ';
                    }
                }
            }
            return outputArray;
        }

        private char[,] PhraseParse(string asOneString, int rows, int columns)
        {
            char[] singleArray = asOneString.ToCharArray();
            return PhraseParse(asOneString.ToCharArray(), rows, columns);
        }
        private char[,] PhraseParse(char[] singleArray, int rows, int columns)
        {
            char[,] finalArray = new char[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    try
                    {
                        finalArray[i, j] = singleArray[(i * columns) + j];
                    }
                    catch
                    {
                        finalArray[i, j] = ' ';
                    }
                    if(finalArray[i, j] == 0)
                    {
                        finalArray[i, j] = ' ';
                    }
                }
            }
            return finalArray;
        }

        private bool PlayAgain()
        {
            guessList = new List<char>();
            phrase = new char[4, 8];
            guesses = new char[3, 8];
            hangman = new char[4, 3];
            board.UpdateGuessesArea(goAgain);
            if(gameWon)
            {
                board.UpdatePhrasesArea(gameOutcomeWin);
            }
            else
            {
                board.UpdatePhrasesArea(gameOutcomeLose);
            }
            board.RefreshBoard();
            char[] input = board.GetBoardInput();
            if(input.Length > 0 && input[0].ToString().ToUpper() == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
