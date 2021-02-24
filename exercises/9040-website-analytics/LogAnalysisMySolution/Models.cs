using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LogAnalysisMySolution
{
    public class Photo
    {
        [JsonPropertyName("pic")]
        public string Picture { get; set; }
    }

    public class LogModel : Photo
    { 
        public DateTime DownloadDateAndTime { get; set; }
    }

    public class Photographer : Photo
    {
        public string takenBy { get; set; }
        public int uploadYear { get; set; }
    }
}
