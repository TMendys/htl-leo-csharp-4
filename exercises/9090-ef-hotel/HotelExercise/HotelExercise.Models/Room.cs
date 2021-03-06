using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelExercise.Models
{
    public class Room
    {
        public int ID { get; set; }
        public int HotelId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public int Size { get; set; }
        public bool DisabilityAccessible { get; set; }
        public int NumberOfRooms { get; set; }
        [Required]
        public RoomPrice RoomPrice { get; set; } = new();
    }
}
