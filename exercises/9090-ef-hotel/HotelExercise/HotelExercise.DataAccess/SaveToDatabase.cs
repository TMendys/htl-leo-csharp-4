using HotelExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelExercise.DataAccess
{
    public class SaveToDatabase
    {
        private static readonly OrderImportFactory factory = new();
        public static async Task Save(List<Hotel> hotels)
        {
            using var context = factory.CreateDbContext();

            await context.AddRangeAsync(hotels);
            await context.SaveChangesAsync();
        }
    }
}
