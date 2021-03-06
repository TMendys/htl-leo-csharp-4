using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace EFChuckNorris
{
    class ChuckNorrisJoke
    {
        public int ID { get; set; }
        public string ChuckNorrisId { get; set; }
        public string Url { get; set; }
        public string Joke { get; set; }
    }
}
