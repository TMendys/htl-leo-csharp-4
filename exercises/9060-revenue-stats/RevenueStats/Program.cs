using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RevenueStats
{
    public record Product(string ProductName, int Amount, int UnitPriceEur, int PriceEur);

    public record Order(int Id, string Customer, string DeliverToCountry)
    {
        public List<Product> Products { get; } = new();
    }

    class Program
    {
        static string[] data;

        static List<Order> orders = new();

        static async Task Main(string[] args)
        {
            await LoadData();

            args = args.Length == 0 ? new[] { "revenue per Customer in percentage" } : args;

            PrintData(args);
        }

        private static void PrintData(string[] args)
        {
            switch (args[0])
            {
                case "summarized revenue":
                    SummarizedRevenue();
                    break;
                case "revenue per Customer":
                    RevenuePerCustomer();
                    break;
                case "revenue per Customer in percentage":
                    RevenuePerCustomerPercentage();
                    break;
            }
        }

        private static void RevenuePerCustomerPercentage()
        {
            var result = orders.GroupBy(x => x.Customer)
                .Distinct().Select(x => new {
                    Customer = x.Key,
                    Sum = x.Sum(x => x.Products.Sum(x => x.PriceEur))
                })
                .OrderByDescending(x => x.Sum);

            var totalRevenue = result.Sum(x => x.Sum);

            foreach (var item in result)
            {
                var percentage = (double)item.Sum / totalRevenue * 100;
                Console.WriteLine($"{item.Customer}: {item.Sum} ({string.Format("{0:0.00}", percentage)} %)");
            }
        }

        private static void RevenuePerCustomer()
        {
            var result = orders.GroupBy(x => x.Customer)
                .Distinct().Select(x => new {
                    Customer = x.Key,
                    Sum = x.Sum(x => x.Products.Sum(x => x.PriceEur))
                })
                .OrderByDescending(x => x.Sum);

            foreach (var item in result)
            {
                Console.WriteLine($"{item.Customer}: {item.Sum}");
            }
        }

        private static void SummarizedRevenue()
        {
            var result = orders.GroupBy(x => x.Id)
                .Select(x => new {
                    Id = x.Key,
                    Sum = x.Sum(x => x.Products.Sum(x => x.PriceEur))
                })
                .OrderByDescending(x => x.Sum);

            foreach (var item in result)
            {
                Console.WriteLine($"{item.Id}: {item.Sum}");
            }
        }

        private static async Task LoadData()
        {
            data = await System.IO.File.ReadAllLinesAsync("order-data.txt");

            for (int i = 2; i < data.Length; i++)
            { 
                if (data[i].Contains("ORDER"))
                {
                    string[] orderLine = data[i].Split("\t");

                    var order = new Order(Convert.ToInt32(orderLine[1]), orderLine[2], orderLine[3]);
                    orders.Add(order);
                }
                else if (data[i].Contains("DETAIL"))
                {
                    string[] productLine = data[i].Split("\t");

                    var product = new Product(
                        productLine[1], 
                        Convert.ToInt32(productLine[2]), 
                        Convert.ToInt32(productLine[3]), 
                        Convert.ToInt32(productLine[4]));

                    orders.Last().Products.Add(product);
                }
            }
        }
    }
}
