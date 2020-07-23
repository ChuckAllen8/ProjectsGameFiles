using System.Collections.Generic;
using System.Text;

namespace FiveCardDraw
{


    class CardHand
    {
        public int MaxReplacable
        {
            get
            {
                foreach (Card card in Cards)
                {
                    if (card.Rank == CRank.Ace)
                    {
                        return 4;
                    }
                }
                return 3;
            }
        }

        public CardHand()
        {
            Cards = new List<Card>();
            SortedCards = new List<Card>();
        }

        public void AddCard(Card card, int index)
        {
            Cards.Insert(index, card);
            SortedCards.Add(card);
            SortedCards.Sort();
            SortedCards.Reverse();
        }

        public List<Card> Cards { get; }

        public List<Card> SortedCards { get; }

        public void RemoveCard(int index)
        {
            SortedCards.Remove(Cards[index]);
            Cards.RemoveAt(index);
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            foreach (Card card in Cards)
            {
                output.Append($"{card} ");
            }
            return output.ToString().Trim();
        }

        public Card this[int index]
        {
            get { return Cards[index]; }
        }

        public int Count
        {
            get { return Cards.Count; }
        }
    }
}
