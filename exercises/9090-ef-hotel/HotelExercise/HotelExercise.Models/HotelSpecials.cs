using System;
using System.Collections.Generic;
using System.Text;

namespace HotelExercise.Models
{
    public enum Special
    {
        Spa,
        Sauna,
        Dog_friendly,
        Indoor_pool,
        Outdoor_pool,
        Bike_rental,
        eCar_charging_station,
        Vegetarian_cuisine,
        Organic_food,
    }
    public class HotelSpecials
    {
        public int ID { get; set; }
        public Special HotelSpecial { get; set; }
    }
}
