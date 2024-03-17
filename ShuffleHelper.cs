using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShuffleCardSample
{
    public static class Precondition
    {
        public static void ThrowOutOfRange(int chunkSize, int lowBound=int.MinValue, int highBound=int.MaxValue)
        {
            if (chunkSize < lowBound || chunkSize > highBound) throw new ArgumentOutOfRangeException(nameof(chunkSize));
        }

        public static void ThrowIfNull<TSource>(TSource source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
        }
    }

    public static class ShuffleHelper
    {
        public static void WriteLine<TSource>(this IEnumerable<TSource> source)
        {
            Precondition.ThrowIfNull(source);

            using var it = source.GetEnumerator();
            while (it.MoveNext())
            {
                Console.Write(it.Current);
                Console.Write(" ");
            }

            Console.WriteLine("\b");
        }

        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source, int k = 15)
        {
            Precondition.ThrowIfNull(source);

            var list = source.ToList();

            Shuffle(list, k);
            
            return list;
        }

        public static void Shuffle<TSource>(this IList<TSource> source, int k = 15)
        {
            Precondition.ThrowIfNull(source);

            var rnd = new Random();
            for (int i = 0; i < k; i++)
            {
                var takeMax = Math.Max(0, source.Count / 3);
                var takeIdx = rnd.Next(takeMax);
                for (int j = takeIdx; j < takeMax; j++)
                {
                    source.Add(source[j]);
                    source.RemoveAt(j);
                }
            }
        }

        public static IEnumerable<int> GenerateCards(int count)
        {
            return Enumerable.Range(1, count);
        }

        public static IEnumerable<Card> GenerateCards()
        {
            string[] suits = new[] {"黑桃", "愛心", "方塊", "梅花" };
            string[] values = new[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            return Enumerable.Range(1, 52)
                .Select((_, i) => new Card(values[i % values.Length], suits[i / values.Length]));
        }

        public static IEnumerable<TSource[]> SendCards<TSource>(this IEnumerable<TSource> cards, int peopleCount)
        {
            Precondition.ThrowIfNull(cards);
            Precondition.ThrowOutOfRange(peopleCount, 1);

            return SendCardsImpl(cards, peopleCount);
        }

        private static IEnumerable<TSource[]> SendCardsImpl<TSource>(IEnumerable<TSource> cards, int peopleCount)
        {
            var list = new List<TSource>();
            for (var k = 0; k < peopleCount; k++)
            {
                using var it = cards.GetEnumerator();
                var i = 0;
                while (it.MoveNext())
                {
                    if (i % peopleCount == k)
                        list.Add(it.Current);
                    i++;
                }

                yield return list.ToArray();
                list.Clear();
            }
        }

        public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int chunkSize)
        {
            Precondition.ThrowIfNull(source);
            Precondition.ThrowOutOfRange(chunkSize, 1);

            return ChunkImpl(source, chunkSize);
        }

        private static IEnumerable<TSource[]> ChunkImpl<TSource>(IEnumerable<TSource> source, int chunkSize)
        {
            using var iterator = source.GetEnumerator();
            var list = new List<TSource>();
            TSource[] tempArray;

            while (iterator.MoveNext())
            {
                var current = iterator.Current;
                list.Add(current);
                if (list.Count == chunkSize)
                {
                    tempArray = list.ToArray();
                    list.Clear();
                    yield return tempArray;
                }
            }

            if (list.Any())
            {
                tempArray = list.ToArray();
                list.Clear();
                yield return tempArray;
            }
        }
    }

}
