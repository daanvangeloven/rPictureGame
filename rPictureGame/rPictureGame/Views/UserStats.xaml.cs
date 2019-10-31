using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Picturegame.Classes;
using Picturegame.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Picturegame.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserStats : ContentPage
    {
        private readonly ApiRequests requests;
        public UserStats()
        {
            InitializeComponent();
            requests = new ApiRequests();
        }

        public async void FillFromUsername(string username)
        {
            UserEntry.Text = username;
            Username.Text = username;
            try
            {
                // GET picturegame api for player
                string results = await requests.MakeGetRequest<string>(
                    "https://api.picturegame.co/leaderboard?players=" + username + "&includeRoundNumbers=false");
                JToken token = JObject.Parse(results)["leaderboard"][0];
                Player mainPlayer = new Player { Username = token["username"].Value<string>(), Rank = token["rank"].Value<string>(), NumWins = token["numWins"].Value<string>() };

                //GET picturegame api for 1st GET rank value - 1
                Player Behind = new Player();
                if (mainPlayer.Rank != "1")
                {
                    results = await requests.MakeGetRequest<string>("https://api.picturegame.co/leaderboard?fromRank=" + (int.Parse(mainPlayer.Rank) - 1) + "&count=1&includeRoundNumbers=false");
                    token = JToken.Parse(results)["leaderboard"][0];
                    Behind = new Player
                        {
                            Username = token["username"].Value<string>(),
                            Rank = token["rank"].Value<string>(),
                            NumWins = token["numWins"].Value<string>()
                        };
                    
                }

                //GET picturegame api for 1st GET rank value + 1
                results = await requests.MakeGetRequest<string>(
                    "https://api.picturegame.co/leaderboard?fromRank=" + (int.Parse(mainPlayer.Rank)+1) +
                    "&count=1&includeRoundNumbers=false");
                token = JToken.Parse(results)["leaderboard"][0];
               
                Player Front = new Player
                    {
                        Username = token["username"].Value<string>(), Rank = token["rank"].Value<string>(),
                        NumWins = token["numWins"].Value<string>()
                    };
                
                // Fill in form
                NumWinsLabel.Text = "Total wins: " + mainPlayer.NumWins;
                RankLabel.Text = "Rank: " + mainPlayer.Rank;
                if (mainPlayer.Rank != "1")
                {
                    PlayerBehind.Text = "Behind: " + Behind.Username + " " + Behind.Rank + "(-" +
                                        (int.Parse(Behind.NumWins) - int.Parse(mainPlayer.NumWins)) + ")";
                }
                // Edge case for number 1 player
                else
                {
                    PlayerBehind.Text = "This is the highest ranking player";
                }
                PlayerInFront.Text = "In front of: " + Front.Username + " " + Front.Rank + "(+" +
                                    (int.Parse(mainPlayer.NumWins) - int.Parse(Front.NumWins)) + ")";
                InfoLayout.IsVisible = true;
                // Zakk easteregg
                if (username.ToLower() == "sodakzak")
                {
                    ZakPic.IsVisible = true;
                }
                else
                {
                    ZakPic.IsVisible = false;
                }
            }
            // Catch, aka can't be bothered to add edgecases
            catch(Exception e)
            {
                await DisplayAlert("Not found",
                    "Couldn't find user. \n Make sure you spelled the name correctly and have a network connection.",
                    "Ok");
                Console.Write("Error: " +e );
            }

        }

        
        private void UserEntry_OnCompleted(object sender, EventArgs e)
        {
            FillFromUsername(UserEntry.Text);
        }
    }
}