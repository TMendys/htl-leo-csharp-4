using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleChart
{
    public class AttackData
    {
        public string Country { get; set; }

        public string TimeOfDay { get; set; }

        public string Animal { get; set; }

        public int NumberOfAttacks { get; set; }
    }

    public static class DataAccess
    {
        readonly static string[] linesData = System.IO.File.ReadAllLines("data.txt");

        public static List<AttackData> AttackDatas()
        {
            List<AttackData> result = new List<AttackData>();

            foreach (var line in linesData)
            {
                if (line == linesData[0])
                    continue;

                string[] lineArray = line.Split("\t");
                AttackData data = new AttackData();
                data.Country = lineArray[0];
                data.TimeOfDay = lineArray[1];
                data.Animal = lineArray[2];
                data.NumberOfAttacks = int.Parse(lineArray[3]);
                result.Add(data);
            }

            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<AttackData> attackDatas = DataAccess.AttackDatas();

            //Makes a group as the Animal as a key and the Number of attacks as the Value. 
            var lines = attackDatas.GroupBy(x => x.Animal)
                .Select(x => new
                { Animal = x.Key, NumberOfAttacks = x.Sum(x => x.NumberOfAttacks) })
                .OrderByDescending(x => x.NumberOfAttacks);

            //See if there is arguments to be passed to the application. 
            //Take the Value to make the list shorter later.
            int result = lines.Count();
            if (args.Length != 0)
                int.TryParse(args[0], out result);

            //Get the max Value of attack and the max Length of the Animal string.
            int maxValue = lines.Max(x => x.NumberOfAttacks);
            int maxStringLength = lines.Max(x => x.Animal.Length);

            foreach (var item in lines.Take(result))
            {
                Console.Write(Bar(maxStringLength, item.Animal.Length, (x, y) => (double)x - y));
                Console.Write($"{item.Animal} | ");
                //Makes a Red bar.
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(Bar(maxValue, item.NumberOfAttacks, (x, y) => (double)y / x * 50 ));
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        //Add spaces for alignment.
        static string Bar(int maxValue, int Length, Func<int, int, double> f) => new string(' ', (int)(f(maxValue, Length)));
    }
}
