using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class CurrentRound : ContentPage
    {
 
        private Round round;

        public CurrentRound()
        {
            InitializeComponent();
            LoadRound();
        }

        private async void LoadRound()
        {
            
            while (true)
            {
                string pictureGameRequest = await new ApiRequests().MakeGetRequest<string>("https://api.picturegame.co/current");
                round = JObject.Parse(pictureGameRequest)["round"].ToObject<Round>();
                string redditGetRequest = await new ApiRequests().MakeGetRequest<string>("https://www.reddit.com/comments/"+ round.Id +"/.json");
                JArray jsonArray = JArray.Parse(redditGetRequest);
                string flairtext = (string)jsonArray.SelectToken("$..data.children[0].data.link_flair_text");
                if (round.Title != CurrentTitle.Text || FlairLabel.Text != flairtext)
                {
                    FlairLabel.Text = flairtext;
                    UsernameLabel.Text = "u/" + round.HostName;
                    UsernameLabel.ClassId = round.HostName;
                    CurrentTitle.Text = round.Title;
                    CurrentImage.Source = round.ImageSource;
                    RedditBtn.IsEnabled = true;
                    Debug.Write("Round has changed");

                }

                await Task.Delay(10000);
                Debug.Write("Refresh current round");


            }
            
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://redd.it/" + round.Id));
        }

        private async void UsernameLabel_OnClicked(object sender, EventArgs e)
        {
            UserStats playermodal = new UserStats();
            playermodal.FillFromUsername(UsernameLabel.ClassId);
            await Navigation.PushModalAsync(playermodal);
        }
    }


   
}