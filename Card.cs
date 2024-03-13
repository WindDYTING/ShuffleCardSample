namespace ShuffleCardSample
{
    public class Card
    {
        public string Number { get; }

        public string Suit { get; }

        public Card(string number, string suit)
        {
            Number = number;
            Suit = suit;
        }

        public override string ToString()
        {
            return $"{Suit}{Number}";
        }
    }
}
