using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.IO;

namespace OrderImport
{
    class OrderImportContext : DbContext
    {
        public OrderImportContext(DbContextOptions<OrderImportContext> options)
            : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
    class OrderImportFactory : IDesignTimeDbContextFactory<OrderImportContext>
    {
        public OrderImportContext CreateDbContext(string[] args = null)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<OrderImportContext>();
            optionsBuilder
                // Uncomment the following line if you want to print generated
                // SQL statements on the console.
                // .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            return new OrderImportContext(optionsBuilder.Options);
        }
    }
    class Customer
    {
        public int ID { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal CreditLimit { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
    class Order
    {
        public int ID { get; set; }
        public int CustomerId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal OrderValue { get; set; }
    }
    class Program
    {
        private static OrderImportFactory factory { get; } = new();

        static async Task Main(string[] args)
        {
            args = args.Length == 0 ? new[] { "FULL", "customers.txt", "orders.txt" } : args;

            switch (args[0].ToUpper())
            {
                case "IMPORT":
                    if(args.Length == 3) await ImportAsync(args[1], args[2]);
                    break;
                case "CLEAN":
                    await CleanAsync();
                    break;
                case "CHECK":
                    await CheckAsync();
                    break;
                case "FULL":
                    if (args.Length == 3) await FullAsync(args[1], args[2]);
                    break;
            }
        }

        private static async Task FullAsync(string customersFile, string ordersFile)
        {
            await CleanAsync();
            await ImportAsync(customersFile, ordersFile);
            await CheckAsync();
        }

        private static async Task CheckAsync()
        {
            var customers = await GetAllCustomersWithOrders();

            var records = customers
                .Where(x => x.CreditLimit < x.Orders.Sum(x => x.OrderValue)).ToArray();

            foreach (var item in records)
            {
                Console.WriteLine(item.Name);
            }
        }

        private static async Task CleanAsync()
        {
            using var dbContext = factory.CreateDbContext();

            dbContext.RemoveRange(await GetAllCustomersWithOrders());
            await dbContext.SaveChangesAsync();
        }

        private static async Task<List<Customer>> GetAllCustomersWithOrders() => 
            await factory.CreateDbContext().Customers
                .Include(x => x.Orders)
                .ToListAsync();
        
        private static async Task ImportAsync(string customersFile, string ordersFile)
        {
            List<Customer> customers = new();

            await ImportCustomers(customersFile, customers);
            await ImportsOrers(ordersFile, customers);
            await SaveToDatabase(customers);
        }

        private static async Task ImportCustomers(string customersFile, List<Customer> customers)
        {
            string[] ordersLines = await File.ReadAllLinesAsync(customersFile);

            for (int i = 1; i < ordersLines.Length; i++)
            {
                string[] oneCustomer = ordersLines[i].Split("\t");

                Customer customer = new() 
                { 
                    Name = oneCustomer[0], 
                    CreditLimit = decimal.Parse(oneCustomer[1]) 
                };

                customers.Add(customer);
            }
        }

        private static async Task ImportsOrers(string ordersFile, List<Customer> customers)
        {
            string[] customersLines = await File.ReadAllLinesAsync(ordersFile);

            for (int i = 1; i < customersLines.Length; i++)
            {
                string[] oneOrder = customersLines[i].Split("\t");

                Order order = new()
                {
                    OrderDate = DateTime.Parse(oneOrder[1]),
                    OrderValue = decimal.Parse(oneOrder[2])
                };

                customers.Where(x => x.Name == oneOrder[0]).FirstOrDefault().Orders.Add(order);
            }
        }

        private static async Task SaveToDatabase(List<Customer> customers)
        {
            using var dbContext = factory.CreateDbContext();

            await dbContext.AddRangeAsync(customers);
            await dbContext.SaveChangesAsync();
        }
    }
}
