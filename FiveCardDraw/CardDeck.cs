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
            Queue<Card> firstHalf = new Queue<Card>();
            Queue<Card> secondHalf = new Queue<Card>();

            for (int index = 0; index < sizeSideOne; index++)
            {
                firstHalf.Enqueue(cards[index]);
            }
            for (int index = sizeSideOne; index < cards.Count; index++)
            {
                secondHalf.Enqueue(cards[index]);
            }
            if (sizeSideOne < 26)
            {
                MergeCards(secondHalf, firstHalf, false);
            }
            else
            {
                MergeCards(firstHalf, secondHalf, true);
            }
        }

        private void CutMiddle(bool flip)
        {
            Queue<Card> firstHalf = new Queue<Card>();
            Queue<Card> secondHalf = new Queue<Card>();

            for (int index = 0; index < 10; index++)
            {
                firstHalf.Enqueue(cards[index]);
            }
            for (int index = 10; index < 35; index++)
            {
                secondHalf.Enqueue(cards[index]);
            }
            for (int index = 35; index < cards.Count; index++)
            {
                firstHalf.Enqueue(cards[index]);
            }

            MergeCards(secondHalf, firstHalf, flip);  
        }

        private void MergeCards(Queue<Card> part1, Queue<Card> part2, bool reverse)
        {
            List<Card> newDeck = new List<Card>();
            while (part1.Count > 0 || part2.Count > 0)
            {
                if (!reverse)
                {
                    if (part1.TryDequeue(out Card one))
                    {
                        newDeck.Add(one);
                    }
                    if (part2.TryDequeue(out Card two))
                    {
                        newDeck.Add(two);
                    }
                }
                else
                {
                    if (part2.TryDequeue(out Card one))
                    {
                        newDeck.Add(one);
                    }
                    if (part1.TryDequeue(out Card two))
                    {
                        newDeck.Add(two);
                    }
                }
            }
            cards = newDeck;
        }
    }
}
