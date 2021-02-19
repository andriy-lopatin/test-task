using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var allData = new List<Symbol>
            {
                new Symbol {Id = 1, Name = "EUR", Qty = 2, Price = 1.56},
                new Symbol {Id = 2, Name = "EUR", Qty = 3, Price = 1.57},
                new Symbol {Id = 3, Name = "USD", Qty = 4, Price = 1.23}
            };

            var aggregates = Aggregate(
                allData, 
                (left, right) => left.Name == right.Name,
                list => new Symbol
                {
                    Name = list.First().Name,
                    Qty = list.Sum(c => c.Qty),
                    Price = list.Average(c => c.Price)
                });
        }

        public static IEnumerable<T> Aggregate<T>(
            IEnumerable<T> data, 
            Func<T, T, bool> shouldGroupBy, 
            Func<IEnumerable<T>, T> calculateAggregate)
        {
            var aggregates = new List<T>();

            foreach (var item in data)
            {
                if (aggregates.Any(a => shouldGroupBy(item, a))) continue;

                // TODO: Optimize loop amount by removing already aggregated entries etc.
                var itemsToAggregate = data.Where(i => shouldGroupBy(item, i));
                aggregates.Add(calculateAggregate(itemsToAggregate));
            }

            return aggregates;
        }
    }
}