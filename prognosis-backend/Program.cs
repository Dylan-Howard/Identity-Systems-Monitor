using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using prognosis_backend.models;
using prognosis_backend;
using Microsoft.Extensions.DependencyInjection;
using prognosis_backend.Controllers;

async System.Threading.Tasks.Task RunAsync(IConfiguration configuration)
{
    Console.WriteLine("Beginning sync tasks");

    try
    {
        // Console.WriteLine("Fetching Profiles from DB");

        string? proUrl = configuration.GetValue<string>("Connections:Prognosis:Url");
        string? proUser = configuration.GetValue<string>("Connections:Prognosis:Username");
        string? proPassword = configuration.GetValue<string>("Connections:Prognosis:Password");
        if (proUrl == null || proUser == null || proPassword == null)
        {
            Console.WriteLine("Unable to load connection settings for Prognosis Database");
            return;
        }

        PrognosisConnectionSettings proConnectionSettings = new PrognosisConnectionSettings {
          Url = proUrl,
          Username = proUser,
          Password = proPassword,
        };

        // List<Profile> profiles = await SyncManager.FetchProfileRecords(proConnectionSettings, 3);

        // if (profiles.Count < 1)
        // {
        //   Console.WriteLine("Sync encountered an error!");
        //   return;
        // }

        // /* Example fetch and process for Rapid Identity */
        // Console.WriteLine("Fetching Users from Rapid Identity");

        // string? riUrl = configuration.GetValue<string>("Connections:RapidIdentity:ApiUrl");
        // string? riUser = configuration.GetValue<string>("Connections:RapidIdentity:Username");
        // string? riPassword = configuration.GetValue<string>("Connections:RapidIdentity:Password");
        // if (riUrl == null || riUser == null || riPassword == null)
        // {
        //     Console.WriteLine("Unable to load connection settings for Rapid Identity");
        //     return;
        // }

        // RapidIdentityConnectionSettings riConnectionSettings = new RapidIdentityConnectionSettings {
        //   Url = riUrl,
        //   Username = riUser,
        //   Password = riPassword,
        // };

        // bool riSyncSuccessful = await SyncManager.SyncRapidIdentity(proConnectionSettings, riConnectionSettings, profiles);

        // if (riSyncSuccessful)
        // {
        //   Console.WriteLine("Sync succeeded!");
        // }
        // else
        // {
        //   Console.WriteLine("Sync encountered an error!");
        //   return;
        // }

        // /* Example fetch and process for Rapid Identity */
        // Console.WriteLine("Fetching Links from Google");
        // bool googleSyncSuccessful = await SyncManager.SyncGoogle(proConnectionSettings);

        // if (googleSyncSuccessful)
        // {
        //   Console.WriteLine("Log succeeded!");
        // }
        // else
        // {
        //   Console.WriteLine("Log encountered an error!");
        //   return;
        // }

        Console.WriteLine("Fetching Linked Accounts from OneRoster");

        string? oneRosterBaseUrl = configuration.GetValue<string>("Connections:OneRoster:BaseUrl");
        string? oneRosterTokenUrl = configuration.GetValue<string>("Connections:OneRoster:TokenUrl");
        string? oneRosterClientId = configuration.GetValue<string>("Connections:OneRoster:ClientId");
        string? oneRosterClientSecret = configuration.GetValue<string>("Connections:OneRoster:ClientSecret");
        if (oneRosterBaseUrl == null || oneRosterTokenUrl == null || oneRosterClientId == null || oneRosterClientSecret == null)
        {
            Console.WriteLine("Unable to load connection settings for OneRoster Api");
            return;
        }

        OneRosterConnectionSettings oneRosterConnectionSettings = new OneRosterConnectionSettings {
            BaseUrl = oneRosterBaseUrl,
            TokenUrl = oneRosterTokenUrl,
            ClientId = oneRosterClientId,
            ClientSecret = oneRosterClientSecret,
        };

        OneRosterController oneRoster = new OneRosterController(oneRosterConnectionSettings);

        bool oneRosterSyncSuccessful = await SyncManager.SyncOneRoster(proConnectionSettings, oneRoster);

        // List<OneRosterOrg> oneRosterOrgs = await oneRoster.FetchOneRosterOrgsAsync();
        // Console.WriteLine($"Received {oneRosterOrgs.Count} orgs.");

        // List<OneRosterUser> oneRosterUsers = await oneRoster.FetchOneRosterUsersAsync();
        // Console.WriteLine($"Received {oneRosterUsers.Count} users.");

        // List<OneRosterClass> oneRosterClasses = await oneRoster.FetchOneRosterClassesAsync();
        // Console.WriteLine($"Received {oneRosterClasses.Count} classes.");
        
        // List<OneRosterEnrollment> oneRosterEnrollments = await oneRoster.FetchOneRosterEnrollmentsAsync();
        // Console.WriteLine($"Received {oneRosterEnrollments.Count} enrollments.");
        
        Console.WriteLine("Sync tasks finished");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    Console.ReadLine();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var configuration = builder.Configuration;

RunAsync(configuration).GetAwaiter().GetResult();