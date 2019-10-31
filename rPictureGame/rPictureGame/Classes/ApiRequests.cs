using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            
                var uri = new Uri(string.Format(resource));
                var response = await _client.GetAsync(uri);
                // if (response.IsSuccessStatusCode)
               // {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;

                    //}

        }
    }

}
