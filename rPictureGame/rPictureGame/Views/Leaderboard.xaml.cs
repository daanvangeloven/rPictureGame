using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Picturegame.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Picturegame.Classes;

namespace Picturegame.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Leaderboard : ContentPage
    {
        private ApiRequests requests;
        ObservableCollection<Player> Players; 
     
        public  Leaderboard()
        {
            InitializeComponent();
            requests = new ApiRequests();
            FillList();
        }

        private async void FillList()
        {
            try
            {
                string Result = await requests.MakeGetRequest<DataTable>("https://api.picturegame.co/leaderboard?count=25&includeRoundNumbers=false");
                List<Player> LeaderList = new List<Player>();
                JToken jToken = JObject.Parse(Result)["leaderboard"];
                int LastPlayer = 0;
                foreach (JToken token in jToken)
                {
                    Player temPlayer = new Player();
                    temPlayer.Rank = token["rank"].Value<string>();
                    Debug.WriteLine(temPlayer.Rank);
                    temPlayer.Username = token["username"].Value<string>();
                    temPlayer.NumWins = token["numWins"].Value<string>();
                    if (LastPlayer == 0)
                    {
                        temPlayer.Difference = "0";
                    }
                    else
                    {
                        temPlayer.Difference = (LastPlayer - token["numWins"].Value<int>()).ToString();
                    }

                    LastPlayer = token["numWins"].Value<int>();



                    LeaderList.Add(temPlayer);
                }
        
                Players = new ObservableCollection<Player>(LeaderList);
                LeaderboardView.ItemsSource = Players;
                UpdateChildrenLayout();
            }    
            catch
            {
                await DisplayAlert("No connection", "Couldn't load leaderboard data", "Ok");
            }
        }

        private void LeaderboardView_OnRefreshing(object sender, EventArgs e)
        {
            FillList();
        }

        private async void Cell_OnTapped(object sender, EventArgs e)
        {
            
            UserStats playermodal = new UserStats();
            playermodal.FillFromUsername(((TextCell)sender).ClassId);
            await Navigation.PushModalAsync(playermodal);
        }
    }

}