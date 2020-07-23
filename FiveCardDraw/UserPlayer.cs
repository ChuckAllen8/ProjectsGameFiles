using System;
using System.Collections.Generic;

namespace FiveCardDraw
{
    class UserPlayer : Player
    {
        public UserPlayer()
        {
            Console.Write("Please enter your name: ");
            Name = Console.ReadLine();
            Hand = new CardHand();
        }

        public override void ReplaceCard(CardDeck deck)
        {
            List<int> cardsReplaced = new List<int>();
            bool replacing = true;
            int maxReplacable = Hand.MaxReplacable;
            Console.WriteLine($"You are allowed to replace {maxReplacable} cards.");
            while (replacing)
            {
                Console.Write("Enter the number (1-5) of a card to replace, or (D)one/(E)nter: ");
                string toReplace = Console.ReadLine();
                int index;
                if (toReplace == "" || toReplace.ToUpper()[0] == 'D' || toReplace.ToUpper()[0] == 'E')
                {
                    Console.WriteLine();
                    break;
                }
                else if (!int.TryParse(toReplace, out index) || !(index >= 1 && index <= 5))
                {
                    Console.WriteLine("That is not a valid selection.");
                    continue;
                }
                else if (cardsReplaced.Contains(index))
                {
                    Console.WriteLine("You have already replaced that card.");
                    continue;
                }
                cardsReplaced.Add(index);
                index--;
                Hand.RemoveCard(index);
                Hand.AddCard(deck.Draw, index);
                if (cardsReplaced.Count == maxReplacable)
                {
                    replacing = false;
                    Console.WriteLine("Maximum cards reached!");
                    Console.WriteLine();
                }
            }
        }
    }
}
