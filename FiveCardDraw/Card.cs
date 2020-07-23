using System;
using System.Diagnostics.CodeAnalysis;

namespace FiveCardDraw
{
    public enum CRank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public enum CSuit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public class Card : IComparable<Card>
    {
        public CRank Rank { get; set; }
        public CSuit Suit { get; set; }

        public Card(CRank rank, CSuit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public override string ToString()
        {
            char display = ' ';
            switch (Suit)
            {
                case CSuit.Clubs:
                    display = '\u2663';
                    break;
                case CSuit.Diamonds:
                    display = '\u2666';
                    break;
                case CSuit.Hearts:
                    display = '\u2665';
                    break;
                case CSuit.Spades:
                    display = '\u2660';
                    break;
            }
            if ((int)Rank < 11)
            {
                return $"{(int)Rank,2}{display}";
            }
            else
            {
                return $"{Rank.ToString()[0],2}{display}";
            }
        }

        public int CompareTo([AllowNull] Card other)
        {
            if (this.Rank == other.Rank)
            {
                return this.Suit.CompareTo(other.Suit);
            }
            else
            {
                return this.Rank.CompareTo(other.Rank);
            }
        }
    }
}
