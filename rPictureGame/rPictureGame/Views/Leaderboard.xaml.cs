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
                // Request top 25 players from api
                string Result = await requests.MakeGetRequest<DataTable>("https://api.picturegame.co/leaderboard?count=25&includeRoundNumbers=false");
                List<Player> LeaderList = new List<Player>();
                JToken jToken = JObject.Parse(Result)["leaderboard"];
                // Int to keep track of last player rank to calculate difference
                int LastPlayer = 0;
                foreach (JToken token in jToken)
                {
                    // Initializa and write object
                    Player temPlayer = new Player();
                    temPlayer.Rank = token["rank"].Value<string>();
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
                // Refresh Listview
                LeaderboardView.ItemsSource = Players;
                UpdateChildrenLayout();
            }    
            // Can't be bothered to add edgecases
            catch(Exception e)
            {
                Debug.WriteLine(e);
                await DisplayAlert("Error", "The app has run into some issues. \n Make sure you have a valid connection or try again later.", "Ok");
            }
        }

        private void LeaderboardView_OnRefreshing(object sender, EventArgs e)
        {        
            FillList();
        }

        private async void Cell_OnTapped(object sender, EventArgs e)
        {
            try
            {
                // Get playerinfo from username and open modal
                UserStats playermodal = new UserStats();
                playermodal.FillFromUsername(((TextCell)sender).ClassId);
                await Navigation.PushModalAsync(playermodal);
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc);
                await DisplayAlert("Error", "The app has run into some issues. \n Make sure you have a valid connection or try again later.", "Ok");
            }
        }
    }

}