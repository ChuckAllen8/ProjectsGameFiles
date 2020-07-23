using System.Collections.Generic;
using System.Linq;

namespace FiveCardDraw
{
    public enum HandType
    {
        HighCard,
        Pair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush
    }

    //
    // Bug obsered when the player has 2 pair that
    // this will call another player with a single pair
    // the winner.
    // Not been able to repeat the bug.
    //
    /* 
     * Two Pair logic did not cover all possible two pair options
     * it also improperly took Value instead of Key for determining
     * which pair had the higher suit, possibly fixed,
     * need to observe again
     * 
     * Observed Two Pair winning correctly after making adjustments,
     * appears to be fixed!
     * 
     */


    public struct HandValue
    {
        public HandType Type { get; set; }

        public List<Card> SortedCards { get; set; }

        public CRank HighestPair { get; set; }

        public CRank LowestPair { get; set; }

        public CRank OffCard { get; set; }

        public CSuit WinningSuit { get; set; }
    }

    class GameRules
    {
        public Player HandWinner(List<Player> players, out HandValue hand)
        {
            List<HandValue> values = new List<HandValue>();

            foreach (Player player in players)
            {
                values.Add(DetermineHand(player.Hand.SortedCards));
            }

            HandValue winningHand = values[0];
            int winningIndex = 0;

            for (int index = 1; index < values.Count; index++)
            {
                if (values[index].Type > winningHand.Type)
                {
                    //the new hand wins
                    winningHand = values[index];
                    winningIndex = index;
                }
                else if (values[index].Type == winningHand.Type)
                {
                    //determine Tie breaker
                    int tieBreaker = TieBreaker(winningHand, values[index]);
                    if (tieBreaker < 0)
                    {
                        winningHand = values[index];
                        winningIndex = index;
                    }
                }
            }
            hand = winningHand;
            return players[winningIndex];
        }

        public Player DetermineWinner(Dictionary<Player, int> scores)
        {
            int winningScore = 0;
            Player winngingPlayer = null;
            foreach (KeyValuePair<Player, int> player in scores)
            {
                if (player.Value > winningScore)
                {
                    winngingPlayer = player.Key;
                    winningScore = player.Value;
                }
            }
            return winngingPlayer;
        }

        private int TieBreaker(HandValue one, HandValue two)
        {
            //1 if the first paramater wins, -1 if the second parameter wins.
            //0 in the event of a tie, favor goes to the left (current winner)
            switch (one.Type)
            {
                case HandType.Flush:
                    return one.WinningSuit.CompareTo(two.WinningSuit);
                case HandType.FullHouse:
                case HandType.FourOfAKind:
                case HandType.ThreeOfAKind:
                case HandType.Straight:
                    return one.HighestPair.CompareTo(two.HighestPair);
                case HandType.StraightFlush:
                    if (one.HighestPair.CompareTo(two.HighestPair) == 0)
                    {
                        return one.WinningSuit.CompareTo(two.WinningSuit);
                    }
                    else
                    {
                        return one.HighestPair.CompareTo(two.HighestPair);
                    }
                case HandType.TwoPair:
                    if (one.HighestPair.CompareTo(two.HighestPair) == 0)
                    {
                        if (one.LowestPair.CompareTo(two.LowestPair) == 0)
                        {
                            return one.OffCard.CompareTo(two.OffCard);
                        }
                        else
                        {
                            return one.LowestPair.CompareTo(two.LowestPair);
                        }
                    }
                    else
                    {
                        return one.HighestPair.CompareTo(two.HighestPair);
                    }
                case HandType.HighCard:
                case HandType.Pair:
                    if (one.HighestPair.CompareTo(two.HighestPair) != 0)
                    {
                        return one.HighestPair.CompareTo(two.HighestPair);
                    }
                    else
                    {
                        for (int index = 0; index < one.SortedCards.Count; index++)
                        {
                            if (one.SortedCards[index].Rank != two.SortedCards[index].Rank)
                            {
                                return one.SortedCards[index].Rank.CompareTo(two.SortedCards[index].Rank);
                            }
                        }
                        return 0;
                    }
                default:
                    return 0;
            }
        }

        public HandValue DetermineHand(List<Card> hand)
        {
            Dictionary<CSuit, int> suits = new Dictionary<CSuit, int>();
            Dictionary<CRank, int> values = new Dictionary<CRank, int>();
            hand.Sort();
            hand.Reverse();
            HandValue val = new HandValue();

            val.SortedCards = hand;

            foreach (Card card in hand)
            {
                if (!suits.TryAdd(card.Suit, 1))
                {
                    suits[card.Suit] = (int)suits[card.Suit] + 1;
                }
                if (!values.TryAdd(card.Rank, 1))
                {
                    values[card.Rank] = (int)values[card.Rank] + 1;
                }
            }

            //must be Full house or Four of a kind.
            if (values.Count == 2)
            {
                //Four of a Kinds
                if (values.ElementAt(0).Value == 4)
                {
                    val.Type = HandType.FourOfAKind;
                    val.HighestPair = values.ElementAt(0).Key;
                    return val;
                }
                else if (values.ElementAt(1).Value == 4)
                {
                    val.Type = HandType.FourOfAKind;
                    val.HighestPair = values.ElementAt(1).Key;
                    return val;
                }

                //Full houses
                if (values.ElementAt(0).Value == 3)
                {
                    val.Type = HandType.FullHouse;
                    val.HighestPair = values.ElementAt(0).Key;
                    val.LowestPair = values.ElementAt(1).Key;
                    return val;
                }
                else if (values.ElementAt(1).Value == 3)
                {
                    val.Type = HandType.FullHouse;
                    val.HighestPair = values.ElementAt(1).Key;
                    val.LowestPair = values.ElementAt(0).Key;
                    return val;
                }
            }

            //Must be either three of a kind or two pair
            if (values.Count == 3)
            {

                //three of a kind
                if (values.ElementAt(0).Value == 3)
                {
                    val.Type = HandType.ThreeOfAKind;
                    val.HighestPair = values.ElementAt(0).Key;
                    return val;
                }
                else if (values.ElementAt(1).Value == 3)
                {
                    val.Type = HandType.ThreeOfAKind;
                    val.HighestPair = values.ElementAt(1).Key;
                    return val;
                }
                else if (values.ElementAt(2).Value == 3)
                {
                    val.Type = HandType.ThreeOfAKind;
                    val.HighestPair = values.ElementAt(2).Key;
                    return val;
                }


                //two pair
                if (values.ElementAt(0).Value == 2 && values.ElementAt(1).Value == 2)
                {
                    val.Type = HandType.TwoPair;
                    if (values.ElementAt(0).Key > values.ElementAt(1).Key)
                    {
                        val.HighestPair = values.ElementAt(0).Key;
                        val.LowestPair = values.ElementAt(1).Key;
                        val.OffCard = values.ElementAt(2).Key;
                    }
                    else
                    {
                        val.HighestPair = values.ElementAt(1).Key;
                        val.LowestPair = values.ElementAt(0).Key;
                        val.OffCard = values.ElementAt(2).Key;
                    }
                    return val;
                }
                else if (values.ElementAt(1).Value == 2 && values.ElementAt(2).Value == 2)
                {
                    val.Type = HandType.TwoPair;
                    if (values.ElementAt(1).Key > values.ElementAt(2).Key)
                    {
                        val.HighestPair = values.ElementAt(1).Key;
                        val.LowestPair = values.ElementAt(2).Key;
                        val.OffCard = values.ElementAt(0).Key;
                    }
                    else
                    {
                        val.HighestPair = values.ElementAt(2).Key;
                        val.LowestPair = values.ElementAt(1).Key;
                        val.OffCard = values.ElementAt(0).Key;
                    }
                    return val;
                }
                else if (values.ElementAt(0).Value == 2 && values.ElementAt(2).Value == 2)
                {
                    val.Type = HandType.TwoPair;
                    if (values.ElementAt(0).Key > values.ElementAt(2).Key)
                    {
                        val.HighestPair = values.ElementAt(0).Key;
                        val.LowestPair = values.ElementAt(2).Key;
                        val.OffCard = values.ElementAt(1).Key;
                    }
                    else
                    {
                        val.HighestPair = values.ElementAt(2).Key;
                        val.LowestPair = values.ElementAt(0).Key;
                        val.OffCard = values.ElementAt(1).Key;
                    }
                    return val;
                }
            }

            //must be a pair
            if (values.Count == 4)
            {
                val.Type = HandType.Pair;
                if (values.ElementAt(0).Value == 2)
                {
                    val.HighestPair = values.ElementAt(0).Key;
                }
                else if (values.ElementAt(1).Value == 2)
                {
                    val.HighestPair = values.ElementAt(1).Key;
                }
                else if (values.ElementAt(2).Value == 2)
                {
                    val.HighestPair = values.ElementAt(2).Key;
                }
                else if (values.ElementAt(3).Value == 2)
                {
                    val.HighestPair = values.ElementAt(3).Key;
                }
                return val;
            }


            bool straight = true;

            for (int index = 1; index < hand.Count; index++)
            {
                if (hand[index].Rank != (hand[index - 1].Rank - 1))
                {
                    straight = false;
                }
            }

            //determine straight
            if (straight)
            {
                val.HighestPair = hand[0].Rank;
                //determine straight flush
                if (suits.Count == 1)
                {
                    val.Type = HandType.StraightFlush;
                    val.WinningSuit = suits.ElementAt(0).Key;
                    return val;
                }
                val.Type = HandType.Straight;
                return val;
            }

            //determine generic flush
            if (suits.Count == 1)
            {
                val.Type = HandType.Flush;
                val.WinningSuit = suits.ElementAt(0).Key;
                return val;
            }
            val.HighestPair = hand[0].Rank;
            val.Type = HandType.HighCard;
            return val;
        }
    }
}
