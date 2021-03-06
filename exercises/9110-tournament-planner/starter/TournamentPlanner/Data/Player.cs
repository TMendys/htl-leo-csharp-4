using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Data
{
    public class Player
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
