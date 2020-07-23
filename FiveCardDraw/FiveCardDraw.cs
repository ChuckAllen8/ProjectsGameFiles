using System;
using System.Collections.Generic;
using System.Text;

namespace FiveCardDraw
{
    public class FiveCardDraw
    {
        private CardDeck deck;
        private const int WIDTH = -25;
        private const int SHUFFLE = 3;
        private List<Player> allPlayers;
        private UserPlayer user;
        private Dictionary<Player, int> scores;
        private bool winner;

        public FiveCardDraw()
        {
            bool settingUp = true;
            while (settingUp)
            {
                Console.SetWindowSize(120, 25);
                Console.WriteLine("Welcome to Five Card Draw!");
                Console.Write("How many players do you want to play with (1-3)? ");
                if (!int.TryParse(Console.ReadLine(), out int playerCount) || !(playerCount >= 1 && playerCount <= 3))
                {
                    Console.WriteLine("Invalid entry, try again.\n");
                    continue; //circle back and try again.
                }

                user = new UserPlayer();
                scores = new Dictionary<Player, int>();
                allPlayers = new List<Player>();
                deck = new CardDeck();

                scores.Add(user, 0);
                allPlayers.Add(user);
                winner = false;

                for (int playerNum = 0; playerNum < playerCount; playerNum++)
                {
                    ComputerPlayer cpu;
                    switch (playerNum)
                    {
                        case 0:
                            cpu = new ComputerPlayer("Mike");
                            break;
                        case 1:
                            cpu = new ComputerPlayer("Riley");
                            break;
                        case 2:
                            cpu = new ComputerPlayer("Jeff");
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    if (cpu.Name == user.Name)
                    {
                        cpu.Name = "Billy Buddy";
                    }
                    allPlayers.Add(cpu);
                    scores.Add(cpu, 0);

                }
                settingUp = false;
            }
        }

        public static void Run(object sender, EventArgs e)
        {
            FiveCardDraw game = new FiveCardDraw();
            game.Game();
        }

        public void Game()
        {
            while (!winner)
            {
                //shuffling takes a moment, do it first
                deck.Shuffle(SHUFFLE);

                //clear the hands, and console before displaying everything.
                ClearHands();
                Console.Clear();

                //show the current scores, deal the cards, show the players hand.
                ShowScoreBoard();
                Deal();
                ShowPlayerHand();

                //Get all replacements, and draw new cards.
                Console.CursorVisible = true;
                Replace();
                Console.CursorVisible = false;
                //determine who wins, and show all the hands
                DetermineHand();
                ShowAllHands();
                winner = isWinner();
            }
            CongratulateWinner(Winner());
            if (GoAgain())
            {
                Game();
            }
        }

        private bool GoAgain()
        {
            Console.Write("Would you like to go again (y/n)? ");
            ConsoleKey selection = Console.ReadKey().Key;
            Console.WriteLine();

            if (selection == ConsoleKey.Y)
            {
                return true;
            }
            else if (selection == ConsoleKey.N)
            {
                return false;
            }
            else
            {
                Console.WriteLine("Invalid selection, try again.");
                return GoAgain();
            }
        }

        private void ShowPlayerHand()
        {
            Console.WriteLine();
            Console.WriteLine(allPlayers[0].Hand.ToString());
            Console.WriteLine();
        }

        private void ShowAllHands()
        {
            StringBuilder handRow = new StringBuilder();
            StringBuilder nameRow = new StringBuilder();
            foreach (KeyValuePair<Player, int> score in scores)
            {
                handRow.Append($"{score.Key.Hand,WIDTH}");
                nameRow.Append($"{score.Key.Name,WIDTH}");
            }
            Console.WriteLine(nameRow);
            Console.WriteLine(handRow);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        private void ClearHands()
        {
            foreach (Player player in allPlayers)
            {
                player.ClearHand();
            }
        }

        private void ShowScoreBoard()
        {
            StringBuilder nameRow = new StringBuilder();
            StringBuilder scoreRow = new StringBuilder();
            foreach (KeyValuePair<Player, int> score in scores)
            {
                nameRow.Append($"{score.Key.Name,WIDTH}");
                scoreRow.Append($"{"Score: " + score.Value,WIDTH}");
            }
            Console.WriteLine(nameRow);
            Console.WriteLine(scoreRow);
        }

        private void Deal()
        {
            for (int cardNum = 1; cardNum <= 5; cardNum++)
            {
                for (int playerNum = 0; playerNum < allPlayers.Count; playerNum++)
                {
                    allPlayers[playerNum].AddCard(deck.Draw);
                }
            }
        }


        private void Replace()
        {

            for (int index = 0; index < allPlayers.Count; index++)
            {
                allPlayers[index].ReplaceCard(deck);
            }
        }

        private void DetermineHand()
        {
            GameRules ruler = new GameRules();
            Player winner = ruler.HandWinner(allPlayers, out HandValue hand);
            scores[winner] = scores[winner] + 1;
            Console.WriteLine($"{winner.Name} wins with {hand.Type}");
            Console.WriteLine();
        }

        private bool isWinner()
        {
            foreach (KeyValuePair<Player, int> score in scores)
            {
                if (score.Value == 10)
                {
                    return true;
                }
            }
            return false;
        }

        private Player Winner()
        {
            GameRules ruler = new GameRules();
            return ruler.DetermineWinner(scores);
        }

        private void CongratulateWinner(Player player)
        {
            Console.Clear();
            ShowScoreBoard();
            Console.WriteLine();
            Console.WriteLine($"{player.Name} wins!!!");
            Console.WriteLine();
        }
    }
}
