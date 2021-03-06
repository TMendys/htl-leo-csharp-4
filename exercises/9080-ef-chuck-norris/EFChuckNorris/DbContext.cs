using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFChuckNorris
{
    class OrderImportContext : DbContext
    {
        public OrderImportContext(DbContextOptions<OrderImportContext> options)
            : base(options)
        { }

        public DbSet<ChuckNorrisJoke> Jokes { get; set; }
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
}
