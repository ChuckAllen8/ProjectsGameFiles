using System;
using System.Collections.Generic;

namespace FiveCardDraw
{
    class CardDeck
    {
        private List<Card> cards;
        private readonly Random random;
        private int topCardIndex;

        public CardDeck()
        {
            random = new Random();
            cards = new List<Card>();

            foreach (CSuit suit in Enum.GetValues(typeof(CSuit)))
            {
                foreach (CRank rank in Enum.GetValues(typeof(CRank)))
                {
                    cards.Add(new Card(rank, suit));
                }
            }
            topCardIndex = cards.Count - 1;
        }

        public Card Draw
        {
            get
            {
                try
                {
                    int oldTop = topCardIndex;
                    topCardIndex--;
                    return cards[oldTop];
                }
                catch
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Shuffle(int times)
        {
            for (int cut = 0; cut <= times; cut++)
            {
                CutMiddle(false);
                CutCards(random.Next(20, 31));
                if (cut % 2 == 0)
                {
                    CutMiddle(true);
                }
            }
            topCardIndex = cards.Count - 1;
        }

        private void CutCards(int sizeSideOne)
        {
            List<Card> firstHalf = new List<Card>();
            List<Card> secondHalf = new List<Card>();

            for (int index = 0; index < sizeSideOne; index++)
            {
                firstHalf.Add(cards[index]);
            }
            for (int index = sizeSideOne; index < cards.Count; index++)
            {
                secondHalf.Add(cards[index]);
            }
            if (sizeSideOne < 26)
            {
                MergeCards(secondHalf, firstHalf);
            }
            else
            {
                MergeCards(firstHalf, secondHalf);
            }
        }

        private void CutMiddle(bool flip)
        {
            List<Card> firstHalf = new List<Card>();
            List<Card> secondHalf = new List<Card>();

            for (int index = 0; index < 10; index++)
            {
                firstHalf.Add(cards[index]);
            }
            for (int index = 10; index < 35; index++)
            {
                secondHalf.Add(cards[index]);
            }
            for (int index = 35; index < cards.Count; index++)
            {
                firstHalf.Add(cards[index]);
            }
            if (flip)
            {
                MergeCards(secondHalf, firstHalf, true);
            }
            else
            {
                MergeCards(firstHalf, secondHalf, true);
            }
        }

        private void MergeCards(List<Card> part1, List<Card> part2)
        {
            List<Card> newDeck = new List<Card>();
            for (int cardNum = 0; cardNum < cards.Count; cardNum++)
            {
                try
                {
                    newDeck.Add(part1[cardNum]);
                }
                catch { }
                try
                {
                    newDeck.Add(part2[cardNum]);
                }
                catch { }
            }
            cards = newDeck;
        }

        private void MergeCards(List<Card> part1, List<Card> part2, bool reverse)
        {
            List<Card> newDeck = new List<Card>();
            for (int cardNum = 0; cardNum < cards.Count; cardNum++)
            {
                try
                {
                    newDeck.Add(part1[part1.Count - 1 - cardNum]);
                }
                catch { }
                try
                {
                    newDeck.Add(part2[part2.Count - 1 - cardNum]);
                }
                catch { }
            }
            cards = newDeck;
        }
    }
}
