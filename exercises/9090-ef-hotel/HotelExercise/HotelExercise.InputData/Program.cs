using HotelExercise.DataAccess;
using HotelExercise.Models;
using System;
using System.Collections.Generic;

namespace HotelExercise.InputData
{
    class Program
    {
        static List<Hotel> hotels = new();
        static void Main(string[] args)
        {
            Console.WriteLine("This program will help you to add hotels and hotel information.");

            bool newHotel = true;
            while (newHotel)
            {
                Hotel hotel = new();

                hotel.Name = HotelName();

                hotel.Address = HotelAddress();

                hotel.AvaibleSpecials = HotelSpecials();

                hotel.Rooms = HotelRooms();

                Console.WriteLine();
                Console.Write("Do You want to add more hotels? (Yes): ");
                newHotel = Console.ReadLine().ToUpper().Equals("YES") ? true : false;
                hotels.Add(hotel);
            }

            SaveToDatabase.Save(hotels);
        }

        private static List<Room> HotelRooms()
        {
            List<Room> rooms = new();
            bool newRooms = true;
            while (newRooms)
            {
                Room room = new();
                Console.WriteLine();
                Console.WriteLine("Add a room.");
                Console.Write("Size in m2: ");
                int.TryParse(Console.ReadLine(), out int size);
                if (size < 1) HotelRooms();
                room.Size = size;

                Console.WriteLine();
                Console.Write("Description: ");
                room.Description = Console.ReadLine();

                Console.WriteLine();
                Console.Write("Prize: ");
                int.TryParse(Console.ReadLine(), out int prize);
                if (prize < 1) HotelRooms();
                room.RoomPrice.Price = prize;

                Console.WriteLine();
                Console.Write("Number of rooms of this type: ");
                int.TryParse(Console.ReadLine(), out int numberOfRooms);
                if (numberOfRooms < 1) HotelRooms();
                room.NumberOfRooms = numberOfRooms;

                Console.WriteLine();
                Console.Write("Do the rooms have disability access? (Yes): ");
                room.DisabilityAccessible = Console.ReadLine().ToUpper().Equals("YES") ? true : false;

                Console.WriteLine();
                Console.Write("Do You want to add more rooms? (Yes): ");
                newRooms = Console.ReadLine().ToUpper().Equals("YES") ? true : false;
                rooms.Add(room);
            }
            return rooms;
        }

        private static List<HotelSpecials> HotelSpecials()
        {
            Console.WriteLine();
            Console.WriteLine("Hotel specials (add ', ' between the specials): ");
            string[] specials = Console.ReadLine().Split(", ");
            List<HotelSpecials> hotelSpecials = new();

            for (int i = 0; i < specials.Length; i++)
            {
                hotelSpecials.Add(specials[i].ToLower() switch
                {
                    "spa"                   => new HotelSpecials { HotelSpecial = Special.Spa },
                    "Sauna"                 => new HotelSpecials { HotelSpecial = Special.Sauna },
                    "dog friendly"          => new HotelSpecials { HotelSpecial = Special.Dog_friendly },
                    "indoor pool"           => new HotelSpecials { HotelSpecial = Special.Indoor_pool },
                    "outdoor pool"          => new HotelSpecials { HotelSpecial = Special.Outdoor_pool },
                    "bike rental"           => new HotelSpecials { HotelSpecial = Special.Bike_rental },
                    "ecar charging station" => new HotelSpecials { HotelSpecial = Special.eCar_charging_station },
                    "vegetarian cuisine"    => new HotelSpecials { HotelSpecial = Special.Vegetarian_cuisine },
                    "organic food"          => new HotelSpecials { HotelSpecial = Special.Organic_food },
                    _ => throw new ArgumentException($"{specials[i]} Doesn't exist as a special.")
                });
            }

            return hotelSpecials;
        }

        private static string HotelAddress()
        {
            Console.WriteLine();
            Console.Write("Hotel Address: ");
            return Console.ReadLine();
        }

        private static string HotelName()
        {
            Console.WriteLine();
            Console.Write("Hotel name: ");
            return Console.ReadLine();
        }
    }
}
