using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Data
{
    public class Match
    {
        public int ID { get; set; }

        [Required]
        public int Round { get; set; }

        [Required]
        public int Player1ID { get; set; }

        [Required]
        public int Player2ID { get; set; }

        public int? WinnerID { get; set; }

        public int GetPlayer(PlayerNumber player) =>
            player switch
            {
                PlayerNumber.Player1 => Player1ID,
                PlayerNumber.Player2 => Player2ID
            };
    }
}