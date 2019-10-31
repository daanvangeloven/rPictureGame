using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Picturegame.Classes
{
    public class Player
    {
        [JsonProperty("rank")]
        public string Rank { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("numWins")]
        public string NumWins { get; set; }
        [JsonProperty("roundList")]
        public List<string> roundList { get; set; }
        public string Difference { get; set; }
        public string RankString => $"{Rank}: {Username}";
        public string WinsDifference => $"Wins: {NumWins} (-{Difference})";
    }

   
}
