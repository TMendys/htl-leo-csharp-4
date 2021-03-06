using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFChuckNorris
{
    class DbController
    {
        private static OrderImportFactory factory { get; } = new();
        private static ChuckNorrisApiController jokeCreator = new();

        public static async Task<List<ChuckNorrisJoke>> CallGetJokesAsync(int number)
        {
            var jokes = await jokeCreator.GetJokesAsync(number);

            return jokes;
        }

        public static async Task<bool> CompareToDb(ChuckNorrisJoke joke)
        {
            var jokesDb = await LoadDb();

            return jokesDb.Exists(x => x.ChuckNorrisId == joke.ChuckNorrisId);
        }

        private static async Task<List<ChuckNorrisJoke>> LoadDb()
        {
            using var dbContext = factory.CreateDbContext();
            return await dbContext.Jokes.ToListAsync();
        }

        public static async Task SaveJokesToDb(int number)
        {
            using var dbContext = factory.CreateDbContext();

            var jokes = await CallGetJokesAsync(number);

            await dbContext.AddRangeAsync(jokes);
            await dbContext.SaveChangesAsync();
        }

        public static async Task ClearDb()
        {
            using var dbContext = factory.CreateDbContext();
            await dbContext.Database.ExecuteSqlRawAsync("DELETE from Jokes");
        }
    }
}
