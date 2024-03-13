using System;
using System.Linq;

namespace ShuffleCardSample
{
    class Program
    {
        static void Main(string[] args)
        {
           //var cards = ShuffleHelper.GenerateCards(52).ToList();   //純數字
            var cards = ShuffleHelper.GenerateCards().ToList(); //有花色

            cards.WriteLine();
            Console.WriteLine();

            cards.Shuffle();
            cards.WriteLine();
            Console.WriteLine();

            var chunkCards = cards.SendCards();
            foreach (var chunkCard in chunkCards)
            {
                chunkCard.WriteLine();
            }

            Console.WriteLine();
            Console.Read();
        }

    }
}
