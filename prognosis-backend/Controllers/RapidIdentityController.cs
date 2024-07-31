using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using prognosis_backend.models;

namespace prognosis_backend
{   
    class RapidIdentityController()
    {
        static HttpClient client = new HttpClient();
        static HttpClient PrepareClient(RapidIdentityConnectionSettings settings)
        {
            client.BaseAddress = new Uri(settings.Url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                .GetBytes(settings.Username + ":" + settings.Password));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

            return client;
        }
        
        public static async Task<List<RapidIdentityUser>> GetUsersAsync(RapidIdentityConnectionSettings settings)
        {
            client = PrepareClient(settings);

            List<RapidIdentityUser> users = [];
            HttpResponseMessage response = await client.GetAsync("users");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<RapidIdentityUsersResponse>();

                if (json != null) {
                    users = json.Data;
                }
            }
            return users;
        }

        public static async Task<RapidIdentityUser?> GetUserAsync(string userId, RapidIdentityConnectionSettings settings)
        {
            client = PrepareClient(settings);

            RapidIdentityUser? user = null;
            HttpResponseMessage response = await client.GetAsync($"users/{userId}");
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