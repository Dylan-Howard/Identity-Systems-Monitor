﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using prognosis_backend;
using Microsoft.Extensions.DependencyInjection;
using prognosis_backend.Controllers;
using prognosis_backend.models;

async System.Threading.Tasks.Task RunAsync(IConfiguration configuration)
{
    Console.WriteLine("Beginning sync tasks");

    try
    {
        /* Initialize Prognosis Connection */
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

        var jobs = await JobsController.GetJobs(proConnectionSettings);
        if (jobs == null)
        {
            Console.WriteLine("Unable to load scheduled jobs");
            return;
        }

        foreach (Job j in jobs)
        {
            Console.WriteLine(j);
        }

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

        // bool riSyncSuccessful = await SyncManager.SyncRapidIdentity(proConnectionSettings, riConnectionSettings);

        // if (riSyncSuccessful)
        // {
        //   Console.WriteLine("Sync succeeded!");
        // }
        // else
        // {
        //   Console.WriteLine("Sync encountered an error!");
        //   return;
        // }

        // /* Example fetch and process for Google */
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

        // /* Example fetch and process for Google */
        // Console.WriteLine("Fetching Linked Accounts from OneRoster");

        // string? oneRosterBaseUrl = configuration.GetValue<string>("Connections:OneRoster:BaseUrl");
        // string? oneRosterTokenUrl = configuration.GetValue<string>("Connections:OneRoster:TokenUrl");
        // string? oneRosterClientId = configuration.GetValue<string>("Connections:OneRoster:ClientId");
        // string? oneRosterClientSecret = configuration.GetValue<string>("Connections:OneRoster:ClientSecret");
        // if (oneRosterBaseUrl == null || oneRosterTokenUrl == null || oneRosterClientId == null || oneRosterClientSecret == null)
        // {
        //     Console.WriteLine("Unable to load connection settings for OneRoster Api");
        //     return;
        // }

        // OneRosterConnectionSettings oneRosterConnectionSettings = new OneRosterConnectionSettings {
        //     BaseUrl = oneRosterBaseUrl,
        //     TokenUrl = oneRosterTokenUrl,
        //     ClientId = oneRosterClientId,
        //     ClientSecret = oneRosterClientSecret,
        // };

        // OneRosterController oneRoster = new OneRosterController(oneRosterConnectionSettings);

        // bool oneRosterSyncSuccessful = await SyncManager.SyncOneRoster(proConnectionSettings, oneRoster);
        
        // Console.WriteLine("Sync tasks finished");
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