using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelExercise.Models
{
    public class RoomPrice
    {
        public int ID { get; set; }
        public int RoomId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        [Required]
        public decimal Price { get; set; }
    }
}
