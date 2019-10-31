using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Picturegame.Services
{
    public class ApiRequests
    {

        HttpClient _client;
        public ApiRequests()
        {
            _client = new HttpClient();
        }

        public async Task<string> MakeGetRequest<T>(string resource)
        {
            try
            {
                var uri = new Uri(string.Format(resource));
                var response = await _client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The app has run into some issues. \n Make sure you have a valid connection or try again later.", "Ok");
                return null;
            }
        }
    }

}
