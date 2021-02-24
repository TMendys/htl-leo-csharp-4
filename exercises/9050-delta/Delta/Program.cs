using System;
using System.Collections.Generic;
using System.Linq;

namespace Delta
{
    class Data
    {
    }

    class Program
    {
        readonly static string[] linesOld = System.IO.File.ReadAllLines("data-old.txt");
        readonly static string[] linesNew = System.IO.File.ReadAllLines("data-new.txt");

        readonly static string[] Header = linesOld[0].Split("\t");

        const int book_iban = 0,
            book_title = 1,
            genre = 2,
            year = 3,
            revenue = 4;

        static void Main(string[] args)
        {
            foreach (var item in Header)
            {
                Console.Write(item + "\t");
            }
            Console.WriteLine();

            Deleted();
            Changed();
            New();
        }

        static void New()
        {
            for (int i = 1; i < linesNew.Length; i++)
            {
                string[] line = linesNew[i].Split("\t");

                var iban = linesOld.Where(x => x.Contains(line[book_iban])).FirstOrDefault();
                var title = linesOld.Where(x => x.Contains(line[book_title])).FirstOrDefault();

                if (string.IsNullOrEmpty(iban) && string.IsNullOrEmpty(title))
                {
                    PrintLine('+', line);
                    Console.WriteLine();
                }
            }
        }

        static void Changed()
        {
            for (int i = 1; i < linesOld.Count(); i++)
            {
                string[] oldLine = linesOld[i].Split("\t");

                var iban = linesNew.Where(x => x.Contains(oldLine[book_iban])).FirstOrDefault();

                if (string.IsNullOrEmpty(iban)) continue;

                if ((!iban.Contains(oldLine[book_title]) && !iban.Contains(oldLine[year]))) continue;
                
                if(!iban.Contains(oldLine[genre]) ||
                    !iban.Contains(oldLine[revenue]))
                {
                    PrintLine('~', iban.Split("\t"));
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        static void Deleted()
        {
            for (int i = 1; i < linesOld.Count(); i++)
            {
                string[] oldLine = linesOld[i].Split("\t");


                if (!linesNew.Any(x => x.Contains(oldLine[book_iban])) &&
                    !linesNew.Any(x => x.Contains(oldLine[book_title])))
                {
                    PrintLine('-',oldLine);
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        static void PrintLine(char prefix, string[] line)
        {
            Console.Write(prefix + " ");
            foreach (var item in line)
            {
                Console.Write($"{item}\t");
            }
        }
    }
}
