using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using HotelExercise.Models;

namespace HotelExercise.DataAccess
{
    class HotelExerciseContext : DbContext
    {
        public HotelExerciseContext(DbContextOptions<HotelExerciseContext> options)
            : base(options)
        { }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelSpecials> HotelSpecials { get; set; }
        public DbSet<RoomPrice> RoomPrices { get; set; }
    }
    class OrderImportFactory : IDesignTimeDbContextFactory<HotelExerciseContext>
    {
        public HotelExerciseContext CreateDbContext(string[] args = null)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<HotelExerciseContext>();
            optionsBuilder
                // Uncomment the following line if you want to print generated
                // SQL statements on the console.
                // .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            return new HotelExerciseContext(optionsBuilder.Options);
        }
    }
}
