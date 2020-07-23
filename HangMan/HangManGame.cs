using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HangMan
{
    class HangManGame
    {
        private int gameWidth, gameHeight;
        private List<char> letterGuesses;
        private char[] currentPhrase;
        private char[] displayPhrase;
        private char[] previousPhrase;
        private List<char[]> screenPhrases;
        private bool stillPlaying, alreadyGuessed, youLost, youWon;
        PhraseMaker phraseFinder;
        HangMan man;
        ConsoleManager gameScreen;
        User player;


        public HangManGame()
        {
            letterGuesses = new List<char>();
            screenPhrases = new List<char[]>();
            gameHeight = 20;
            gameWidth = 45;
            man = new HangMan();
            gameScreen = new ConsoleManager();
            phraseFinder = new PhraseMaker();
            player = new User(gameScreen);
            gameScreen.ConsoleResize(gameHeight, gameWidth);
            stillPlaying = true;
            alreadyGuessed = false;
            youLost = youWon = false;

            screenPhrases.Add(new char[] { 'G', 'U', 'E', 'S', 'S', ' ', 'T', 'H', 'E', ' ', 'P', 'H', 'R', 'A', 'S', 'E' });
            screenPhrases.Add(new char[] { 'P', 'R', 'I', 'O', 'R', ' ', 'G', 'U', 'E', 'S', 'S', 'E', 'S' });
            screenPhrases.Add(new char[] { 'A', 'L', 'R', 'E', 'A', 'D', 'Y', ' ', 'T', 'R', 'I', 'E', 'D' });
            screenPhrases.Add(new char[] { 'Y', 'O', 'U', ' ', 'L', 'O', 'S', 'T', '!' });
            screenPhrases.Add(new char[] { 'Y', 'O', 'U', ' ', 'W', 'I', 'N', '!' });
            screenPhrases.Add(new char[] { 'P', 'L', 'A', 'Y', ' ', 'A', 'G', 'A', 'I', 'N', '?' });
        }

        public void Start()
        {
            currentPhrase = phraseFinder.GetPhrase();
            ResetDisplayPhrase();
            UpdateConsole();

            while(!youLost && !youWon && stillPlaying)
            {
                UpdateConsole();
                //get user guess
                player.GetGuess();
                if (player.IsPhraseGuess)
                {
                    CheckPhraseGuess();
                }
                else
                {
                    CheckLetterGuess();
                }


                if(youLost || youWon)
                {
                    UpdateConsole();
                    stillPlaying = player.PlayAgain();
                    youLost = youWon = false;
                    currentPhrase = phraseFinder.GetPhrase();
                    letterGuesses = new List<char>();
                    ResetDisplayPhrase();
                    man.ResetHangMan();
                }
            }
        }

        private void CheckLetterGuess()
        {
            if (letterGuesses.Contains(player.Guess[0]))
            {
                alreadyGuessed = true;
            }
            else
            {
                alreadyGuessed = false;
                letterGuesses.Add(player.Guess[0]);
                bool success = false;
                displayPhrase.CopyTo(previousPhrase, 0);
                UpdateDisplayPhrase(player.Guess[0]);
                for (int i = 0; i < displayPhrase.Length; i++)
                {
                    if (displayPhrase[i] != previousPhrase[i])
                    {
                        success = true;
                    }
                }

                youWon = true;
                for (int i = 0; i < currentPhrase.Length; i++)
                {
                    if (displayPhrase[i] != currentPhrase[i])
                    {
                        youWon = false;
                    }
                }

                if (!success)
                {
                    man.AddWrongGuess();
                    if(!man.IsAlive)
                    {
                        youLost = true;
                    }
                }
            }
        }

        private void CheckPhraseGuess()
        {
            if (player.Guess.Length == currentPhrase.Length)
            {
                for (int i = 0; i < currentPhrase.Length; i++)
                {
                    if(player.Guess[i] != currentPhrase[i])
                    {
                        youLost = true;
                    }
                }
            }
            else
            {
                youLost = true;
            }
            if(!youLost)
            {
                youWon = true;
            }
        }

        private void UpdateDisplayPhrase(char letter)
        {
            for(int i = 0; i < displayPhrase.Length; i++)
            {
                if(currentPhrase[i] == letter)
                {
                    displayPhrase[i] = letter;
                }
            }
        }

        private void ResetDisplayPhrase()
        {
            displayPhrase = new char[currentPhrase.Length];
            for(int i = 0; i < displayPhrase.Length; i++)
            {
                if (currentPhrase[i] == ' ')
                {
                    displayPhrase[i] = currentPhrase[i];
                }
                else
                {
                    displayPhrase[i] = '_';
                }
            }
            previousPhrase = new char[currentPhrase.Length];
        }

        private void UpdateConsole()
        {
            char[,] updatedScreen = new char[gameHeight, gameWidth];
            char[,] updatedHangMan = man.GetHangMan();

            for (int i = 0; i < screenPhrases[0].Length; i++)
            {
                updatedScreen[8, i] = screenPhrases[0][i];
            }
            for (int i = 0; i < screenPhrases[1].Length; i++)
            {
                updatedScreen[14, i] = screenPhrases[1][i];
            }

            for (int i = 0; i < 7; i++) //display the hangmans noose and current hangman
            {
                for (int j = 0; j < 7; j++)
                {
                    updatedScreen[i, j] = updatedHangMan[i, j];
                }
            }

            //add in the current phrase
            for (int i = 0; i < displayPhrase.Length; i++)
            {
                updatedScreen[12, i] = displayPhrase[i];
            }
            for(int i = 0; i < gameWidth; i++)
            {
                updatedScreen[15, i] = ' ';
            }
            for (int i = 0; i < letterGuesses.Count; i++)
            {
                updatedScreen[15, i * 2 + 1] = letterGuesses[i];
            }

            //Inform if the letter was already guessed
            if(alreadyGuessed)
            {
                for (int i = 0; i < screenPhrases[2].Length; i++)
                {
                    updatedScreen[17, i] = screenPhrases[2][i];
                }
            }

            //if the game is over let them know if they won or lost, and ask about playing again
            if (youWon || youLost)
            {
                if (youLost)
                {
                    for (int i = 0; i < screenPhrases[3].Length; i++)
                    {
                        updatedScreen[2, i+10] = screenPhrases[3][i];
                    }
                }
                else
                {
                    for (int i = 0; i < screenPhrases[4].Length; i++)
                    {
                        updatedScreen[2, i+10] = screenPhrases[4][i];
                    }
                }
                for (int i = 0; i < screenPhrases[5].Length; i++)
                {
                    updatedScreen[3, i+10] = screenPhrases[5][i];
                }
            }

            gameScreen.ConsoleUpdate(updatedScreen);
        }
    }
}
