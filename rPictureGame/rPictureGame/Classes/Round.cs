using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Picturegame.Classes
{
    public class Round
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("postUrl")]
        public string ImageSource { get; set; }
        [JsonProperty("hostName")]
        public string HostName { get; set; }
    }
}
