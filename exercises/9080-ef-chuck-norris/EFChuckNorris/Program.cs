using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EFChuckNorris
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await DbController.ClearDb();
            //await DbController.SaveJokesToDb(10);
        }
    }
}
