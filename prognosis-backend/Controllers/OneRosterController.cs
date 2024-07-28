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
    // [Route("api/[controller]")]
    // [ApiController]
    public class OneRosterController : ControllerBase
    {
        private readonly PrognosisContext _context;

        public OneRosterController(PrognosisContext context)
        {
            _context = context;
        }

        static HttpClient client = new HttpClient();
        public static async Task<string> GetAccessToken(OneRosterConnectionSettings settings)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", settings.ClientId),
                new KeyValuePair<string, string>("client_secret", settings.ClientSecret)
            });

            var response = await client.PostAsync(settings.TokenUrl, content);
            response.EnsureSuccessStatusCode();

            OneRosterTokenResponse tokenResponse = await response.Content.ReadFromJsonAsync<OneRosterTokenResponse>();

            if (tokenResponse != null)
            {
                return tokenResponse.AccessToken;  
            }

            return "failed";
        }
        static async Task<HttpClient> PrepareClient(OneRosterConnectionSettings settings)
        {
            string accessToken = await GetAccessToken(settings);

            if (accessToken == "failed")
            {
                return null;
            }

            client.BaseAddress = new Uri(settings.BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return client;
        }

        public static async Task<List<OneRosterUser>> FetchUsersAsync(OneRosterConnectionSettings settings)
        {
            client = await PrepareClient(settings);

            if (client == null)
            {
                return null;
            }

            List<OneRosterUser> users = [];
            HttpResponseMessage response = await client.GetAsync("enrollments");

            Console.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<OneRosterEnrollmentsResponse>();
                Console.WriteLine(json.Enrollments.FirstOrDefault());

                // var json = await response.Content.ReadAsStringAsync();
                // Console.WriteLine(json);

                // if (json != null) {
                //     users = json.Orgs;
                // }
            }
            return users;
        }

        // // GET: api/Agents
        // [HttpGet]
        // public async Task<ActionResult<AgentList>> GetAgent()
        // {
        //     List<Agent> agents = await _context.Agents.ToListAsync();
        //     return new AgentList {
        //       Agents = agents,
        //       Total = agents.Count,
        //     };
        // }

        // // GET: api/Agents/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Agent>> GetAgent(Guid id)
        // {
        //     var agent = await _context.Agents.FindAsync(id);

        //     if (agent == null)
        //     {
        //         return NotFound();
        //     }

        //     return agent;
        // }

    }
}
