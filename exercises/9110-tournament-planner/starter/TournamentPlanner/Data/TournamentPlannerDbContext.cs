using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TournamentPlanner.Data
{
    public enum PlayerNumber { Player1 = 1, Player2 = 2 };

    public class TournamentPlannerDbContext : DbContext
    {
        public TournamentPlannerDbContext(DbContextOptions<TournamentPlannerDbContext> options)
            : base(options)
        { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Match>()
        //}

        /// <summary>
        /// Adds a new player to the player table
        /// </summary>
        /// <param name="newPlayer">Player to add</param>
        /// <returns>Player after it has been added to the DB</returns>
        public async Task<Player> AddPlayer(Player newPlayer)
        {
            Add(newPlayer);
            await SaveChangesAsync();
            return newPlayer;
        }

        /// <summary>
        /// Adds a match between two players
        /// </summary>
        /// <param name="player1Id">ID of player 1</param>
        /// <param name="player2Id">ID of player 2</param>
        /// <param name="round">Number of the round</param>
        /// <returns>Generated match after it has been added to the DB</returns>
        public async Task<Match> AddMatch(int player1Id, int player2Id, int round)
        {
            Match match = new ()
            {
                Player1ID = player1Id,
                Player2ID = player2Id,
                Round = round
            };

            Add(match);
            await SaveChangesAsync();

            return match;
        }

        /// <summary>
        /// Set winner of an existing game
        /// </summary>
        /// <param name="matchId">ID of the match to update</param>
        /// <param name="player">Player who has won the match</param>
        /// <returns>Match after it has been updated in the DB</returns>
        public async Task<Match> SetWinner(int matchId, PlayerNumber player)
        {
            Match match = Matches.Find(matchId);
            match.WinnerID = match.GetPlayer(player);

            await SaveChangesAsync();

            return match;
        }

        /// <summary>
        /// Get a list of all matches that do not have a winner yet
        /// </summary>
        /// <returns>List of all found matches</returns>
        public async Task<IList<Match>> GetIncompleteMatches() => await Matches.Where(m => m.WinnerID == null).ToListAsync();
        

        /// <summary>
        /// Delete everything (matches, players)
        /// </summary>
        public async Task DeleteEverything()
        {
            await Database.ExecuteSqlRawAsync("DELETE from Matches");
            await Database.ExecuteSqlRawAsync("DELETE from Players");
        }

        /// <summary>
        /// Get a list of all players whose name contains <paramref name="playerFilter"/>
        /// </summary>
        /// <param name="playerFilter">Player filter. If null, all players must be returned</param>
        /// <returns>List of all found players</returns>
        public async Task<IList<Player>> GetFilteredPlayers(string playerFilter = null) =>
            await Players.Where(p => playerFilter == null || p.Name.Contains(playerFilter)).ToListAsync();


        /// <summary>
        /// Generate match records for the next round
        /// </summary>
        /// <exception cref="InvalidOperationException">Error while generating match records</exception>
        public async Task GenerateMatchesForNextRound()
        {
            if ((await Players.CountAsync()) != 32 || (await GetIncompleteMatches()).Count < 0)
                throw new InvalidOperationException();

            if (await Matches.AnyAsync())
                await GenerateNextRound();
            else
                await GenerateFirstRound();
        }

        internal async Task GenerateFirstRound()
        {
            List<Player> players = await Players.ToListAsync();

            for (int i = 0; i < players.Count; i += 2)
            {
                await AddMatch(players[i].ID, players[i + 1].ID, 1);
            }
        }

        private async Task GenerateNextRound()
        {
            List<Match> matches = await Matches.ToListAsync();
            List<Player> players = await Players.ToListAsync();

            int lastRound =  matches.Max(m => m.Round);
            List<Player> playersForNextRound = new();

            foreach (Match match in matches.Where(m => m.Round == lastRound))
            {
                playersForNextRound.Add(players.FirstOrDefault(p => p.ID == match.WinnerID));
            }

            for (int i = 0; i < playersForNextRound.Count; i += 2)
            {
                await AddMatch(playersForNextRound[i].ID, playersForNextRound[i + 1].ID, lastRound + 1);
            }
        }
    }
}
