using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using prognosis_backend.models;

namespace prognosis_backend.Controllers
{   
    class RapidIdentityController : ControllerBase
    {
        private RapidIdentityConnectionSettings _connectionSettings;
        private HttpClient? _client;
        public int fetchLimit;
        public RapidIdentityController(RapidIdentityConnectionSettings settings)
        {
            _connectionSettings = settings;
            fetchLimit = 500;
        }
        private HttpClient? PrepareClient()
        {
            if (_client != null)
            {
                return _client;
            }

            _client = new HttpClient
            {
                BaseAddress = new Uri(_connectionSettings.Url)
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                .GetBytes(_connectionSettings.Username + ":" + _connectionSettings.Password));
            _client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

            return _client;
        }
        
        public async Task<List<RapidIdentityUser>> GetUsersAsync()
        {
            _client = PrepareClient();

            if (_client == null)
            {
                return [];
            }

            List<RapidIdentityUser> users = [];
            HttpResponseMessage response = await _client.GetAsync("users");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<RapidIdentityUsersResponse>();

                if (json != null) {
                    users = json.Data;
                }
            }
            return users;
        }

        public async Task<RapidIdentityUser?> GetUserAsync(string userId)
        {
            _client = PrepareClient();

            if (_client == null)
            {
                return null;
            }

            RapidIdentityUser? user = null;
            HttpResponseMessage response = await _client.GetAsync($"users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<RapidIdentityUserResponse>();

                if (json != null) {
                  user = json.Data;
                }
            }
            return user;
        }

    }

    class RapidIdentityConnectionSettings
    {
        public required string Url { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}