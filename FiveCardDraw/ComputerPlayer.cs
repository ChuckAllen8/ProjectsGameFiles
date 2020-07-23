using System.Collections.Generic;

namespace FiveCardDraw
{
    class ComputerPlayer : Player
    {
        public ComputerPlayer(string name)
        {
            Name = name;
            Hand = new CardHand();
        }

        public override void ReplaceCard(CardDeck deck)
        {
            GameRules rules = new GameRules();
            HandValue myVal = rules.DetermineHand(Hand.SortedCards);
            List<int> replace = new List<int>();

            switch (myVal.Type)
            {
                case HandType.HighCard: //replace the three smallest cards
                    for (int index = Hand.Count - 1; index > 1; index--)
                    {
                        replace.Add(Hand.Cards.IndexOf(Hand.SortedCards[index]));
                    }
                    break;
                case HandType.Pair:
                    for (int index = 0; index < Hand.Count; index++)
                    {
                        if (Hand.Cards[index].Rank != myVal.HighestPair)
                        {
                            replace.Add(index);
                        }
                    }
                    break;
                case HandType.TwoPair:
                    for (int index = 0; index < Hand.Count; index++)
                    {
                        if (Hand.Cards[index].Rank != myVal.HighestPair && Hand.Cards[index].Rank != myVal.LowestPair)
                        {
                            replace.Add(index);
                        }
                    }
                    break;
                case HandType.ThreeOfAKind:
                    for (int index = 0; index < Hand.Count; index++)
                    {
                        if (Hand.Cards[index].Rank != myVal.HighestPair)
                        {
                            replace.Add(index);
                        }
                    }
                    break;
                default:
                    return;
            }
            foreach (int cardIndex in replace)
            {
                Hand.RemoveCard(cardIndex);
                Hand.AddCard(deck.Draw, cardIndex);
            }
        }
    }
}
