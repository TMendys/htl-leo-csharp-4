using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogAnalysisMySolution
{
    public class LogReader
    {
        public List<LogModel> LogModels { get; set; } = new();
        public List<Photographer> Photographers { get; set; }

        public LogReader()
        {
            LoadAccessLog();

            LoadPhotographers();
        }

        private void LoadAccessLog()
        {
            string[] linesData = File.ReadAllLines("access-log.txt");

            foreach (var line in linesData)
            {
                if (line == linesData[0]) continue;

                string[] lineArray = line.Split("\t");

                LogModel log = new LogModel
                {
                    Picture = lineArray[0],
                    DownloadDateAndTime = DateTime.Parse($"{lineArray[1]} {lineArray[2]}")
                };

                LogModels.Add(log);
            }
        }

        private void LoadPhotographers()
        {
            string photographers = File.ReadAllText("photographers.json");
            Photographers = JsonSerializer.Deserialize<List<Photographer>>(photographers); 
        }

        public void SummarizeNumberOfDownloads(string[] args)
        {
            switch (args[0])
            {
                case "monthly":
                    MonthlyOption();
                    break;
                case "hourly":
                    HourlyOption();
                    break;
                case "photographers":
                    PhotographersOption();
                    break;

            }
        }

        private void MonthlyOption()
        {
            var result = LogModels.GroupBy(x => x.Picture)
                .Select(x => new {
                    Photo = x.Key,
                    Dates = x.AsEnumerable()
                        .GroupBy(x => x.DownloadDateAndTime
                        .ToString("yyyy-MM:").ToString())
                });

            foreach (var item in result)
            {
                Console.WriteLine(item.Photo);

                foreach (var date in item.Dates)
                {
                    Console.WriteLine($"\t{date.Key} {date.Count()}");
                }
            }
        }

        private void HourlyOption()
        {
            var result = LogModels.GroupBy(x => x.Picture)
                .OrderBy(x => x.Key)
                .Select(x => new
                {
                    Photo = x.Key,
                    Time = x.AsEnumerable()
                        .GroupBy(x => x.DownloadDateAndTime
                        .ToString("HH:00:"))
                        .OrderBy(x => x.Key)
                });

            foreach (var item in result)
            {
                Console.WriteLine(item.Photo);

                int numberOfPhotosPerPhoto = item.Time.Sum(x => x.Count());

                foreach (var time in item.Time)
                {
                    int numberOfPhotosPerHour = time.Count();
                    Console.Write($"\t{time.Key} ");
                    Console.WriteLine($"{string.Format("{0:0.00}", (decimal)numberOfPhotosPerHour / numberOfPhotosPerPhoto * 100)} %");
                }
            }
        }

        private void PhotographersOption()
        {
            var logResult = LogModels.GroupBy(x => x.Picture);

            var result = Photographers.Join(logResult, 
                photographer => photographer.Picture,
                logResult => logResult.Key,
                (photographer, logResult) =>
                    new { Name = photographer.takenBy, NumberOfPictures = logResult.Count() })
                        .OrderByDescending(x => x.NumberOfPictures);


            foreach (var item in result)
            {
                Console.WriteLine($"{item.Name}: {item.NumberOfPictures}");
            }
        }
    }
}
