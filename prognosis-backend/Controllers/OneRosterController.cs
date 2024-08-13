using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using Prognosis.Models;
using prognosis_backend.models;

namespace prognosis_backend.Controllers
{
    public class OneRosterController : ControllerBase
    {
        private OneRosterConnectionSettings _connectionSettings;
        private HttpClient? _client;
        public int fetchLimit;
        readonly int _progressIncrement = 500;
        public OneRosterController(OneRosterConnectionSettings settings)
        {
            _connectionSettings = settings;
            fetchLimit = 100;
        }
        
        public async Task<string> GetAccessToken()
        {
            HttpClient client = new HttpClient();
            
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _connectionSettings.ClientId),
                new KeyValuePair<string, string>("client_secret", _connectionSettings.ClientSecret)
            });

            var response = await client.PostAsync(_connectionSettings.TokenUrl, content);
            response.EnsureSuccessStatusCode();

            OneRosterTokenResponse? tokenResponse = await response.Content.ReadFromJsonAsync<OneRosterTokenResponse>();

            if (tokenResponse != null)
            {
                return tokenResponse.AccessToken;  
            }

            return "failed";
        }
        private async Task<HttpClient?> PrepareClient()
        {
            if (_client != null)
            {
                return _client;
            }

            string accessToken = await GetAccessToken();

            if (accessToken == "failed")
            {
                return null;
            }

            _client = new HttpClient
            {
                BaseAddress = new Uri(_connectionSettings.BaseUrl)
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return _client;
        }
        public async Task<List<OneRosterUser>> FetchOneRosterUsersAsync()
        {
            _client = await PrepareClient();

            if (_client == null)
            {
                return [];
            }

            List<OneRosterUser> users = [];
            HttpResponseMessage response;
            OneRosterUsersResponse? json;
            int offsetIndex = 0;
            int lastItemCount = fetchLimit;

            do
            {
                response = await _client.GetAsync($"users?limit={fetchLimit}&offset={offsetIndex}");
                
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadFromJsonAsync<OneRosterUsersResponse>();
                    if (json != null) {
                        users.AddRange(json.Users);
                        if (users.Count % _progressIncrement == 0)
                        {
                            Console.WriteLine($"Fetched {json.Users.Count} users. {users.Count} total now.");
                        }
                    }
                    offsetIndex += json != null ? json.Users.Count : fetchLimit;
                    lastItemCount = json != null ? json.Users.Count : 0;
                }
                else
                {
                  lastItemCount = 0;
                }
            }
            while (fetchLimit <= lastItemCount);

            return users;
        }
        public async Task<List<OneRosterOrg>> FetchOneRosterOrgsAsync()
        {
            _client = await PrepareClient();

            if (_client == null)
            {
                return [];
            }

            List<OneRosterOrg> orgs = [];
            HttpResponseMessage response = await _client.GetAsync("orgs");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<OneRosterOrgsResponse>();

                if (json != null) {
                    orgs = json.Orgs;
                }
            }
            return orgs;
        }
        public async Task<List<OneRosterClass>> FetchOneRosterClassesAsync()
        {
            _client = await PrepareClient();

            if (_client == null)
            {
                return [];
            }

            List<OneRosterClass> classes = [];
            HttpResponseMessage response;
            OneRosterClassesResponse? json;
            int offsetIndex = 0;
            int lastItemCount = fetchLimit;

            do
            {
                response = await _client.GetAsync($"classes?limit={fetchLimit}&offset={offsetIndex}");
                
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadFromJsonAsync<OneRosterClassesResponse>();
                    if (json != null) {
                        classes.AddRange(json.Classes);
                        if (classes.Count % _progressIncrement == 0)
                        {
                            Console.WriteLine($"Fetched {json.Classes.Count} classes. {classes.Count} total now.");
                        }
                    }
                    offsetIndex += json != null ? json.Classes.Count : fetchLimit;
                    lastItemCount = json != null ? json.Classes.Count : 0;
                }
                else
                {
                  lastItemCount = 0;
                }
            }
            while (fetchLimit <= lastItemCount);
            return classes;
        }
        public async Task<List<OneRosterEnrollment>> FetchOneRosterEnrollmentsAsync()
        {
            _client = await PrepareClient();

            if (_client == null)
            {
                return [];
            }

            List<OneRosterEnrollment> enrollments = [];
            HttpResponseMessage response;
            OneRosterEnrollmentsResponse? json;
            int offsetIndex = 0;
            int lastFetchCount = fetchLimit;

            do
            {
                response = await _client.GetAsync($"enrollments?limit={fetchLimit}&offset={offsetIndex}");
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadFromJsonAsync<OneRosterEnrollmentsResponse>();
                    if (json != null) {
                        enrollments.AddRange(json.Enrollments);
                        if (enrollments.Count % _progressIncrement == 0)
                        {
                            Console.WriteLine($"Fetched {json.Enrollments.Count} enrollments. {enrollments.Count} total now.");
                        }
                    }
                    offsetIndex += json != null ? json.Enrollments.Count : fetchLimit;
                    lastFetchCount = json != null ? json.Enrollments.Count : 0;
                }
                else
                {
                    lastFetchCount = 0;
                }
            }
            while (fetchLimit <= lastFetchCount);

            return enrollments;
        }
    }
}
