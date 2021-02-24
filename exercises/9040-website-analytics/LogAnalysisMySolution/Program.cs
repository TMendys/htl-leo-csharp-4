using System;
using System.Collections.Generic;

namespace LogAnalysisMySolution
{
    class Program
    {
        static void Main(string[] args)
        {
            LogReader logReader = new LogReader();

            if(args.Length == 0) args = new string[] { "photographers" };

            logReader.SummarizeNumberOfDownloads(args);
        }
    }
}
