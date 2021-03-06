using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelExercise.Models
{
    public class Hotel
    {
        public int ID { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        //Street, ZIP Code, City
        [MaxLength(150)]
        [Required]
        public string Address { get; set; }
        public List<HotelSpecials> AvaibleSpecials { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
