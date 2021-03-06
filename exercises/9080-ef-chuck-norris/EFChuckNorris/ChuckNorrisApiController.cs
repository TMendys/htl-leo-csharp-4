using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace EFChuckNorris
{
    class ChuckNorrisApiController
    {
        private HttpClient httpClient = new();
        public async Task<ChuckNorrisJoke> GetJokesAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.chucknorris.io/jokes/random");
            using var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {

                var loadAJoke = await response.Content.ReadAsStringAsync();

                var document = JsonDocument.Parse(loadAJoke);
                JsonElement root = document.RootElement;

                if (root.GetProperty("categories").ToString().Contains("explicit")) await GetJokesAsync();

                var joke = new ChuckNorrisJoke()
                {
                    ChuckNorrisId = root.GetProperty("id").ToString(),
                    Joke = root.GetProperty("value").ToString(),
                    Url = root.GetProperty("url").ToString()
                };

                if (await DbController.CompareToDb(joke)) await GetJokesAsync();

                return joke;
            }

            return new ChuckNorrisJoke();
        }

        public async Task<List<ChuckNorrisJoke>> GetJokesAsync(int numberOfJokes)
        {
            List<ChuckNorrisJoke> jokes = new();

            for (int i = 0; i < numberOfJokes; i++)
            {
                ChuckNorrisJoke joke;

                do { joke = await GetJokesAsync(); } 
                while (jokes.Contains(joke));

                jokes.Add(joke);
            }

            return jokes;
        }
    }
}
