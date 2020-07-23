namespace FiveCardDraw
{
    class Player
    {
        public string Name { get; set; }
        public CardHand Hand { get; set; }

        public virtual void ReplaceCard(CardDeck deck)
        { }

        public void ClearHand()
        {
            while (Hand.Count > 0)
            {
                Hand.RemoveCard(0);
            }
        }

        public void AddCard(Card card)
        {
            Hand.AddCard(card, Hand.Count);
        }
    }
}
