using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using prognosis_backend;
using Microsoft.Extensions.DependencyInjection;
using prognosis_backend.Controllers;
using prognosis_backend.models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;



async System.Threading.Tasks.Task RunAsync(IConfiguration configuration)
{
    Console.WriteLine("Checking for ready jobs");

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

        var jobs = await JobsController.GetReadyJobs(proConnectionSettings);
        if (jobs == null)
        {
            Console.WriteLine("Unable to load scheduled jobs");
            return;
        }

        foreach (Job j in jobs)
        {
            /* Update the job's next runtime */
            var now = DateTime.Now;
            j.NextRuntime = DateTime.Now.AddMinutes(Double.Parse(j.Frequency));
            await JobsController.PutJob(proConnectionSettings, j);

            /* Create a new task */
            Console.WriteLine($"Creating task for {j.ServiceId}");
            prognosis_backend.models.Task? currTask = await TasksController.PostTask(proConnectionSettings, new prognosis_backend.models.Task {
                JobId = j.JobId,
                StartTime = DateTime.Now,
                Active = true,
            });

            if (currTask == null)
            {
                continue;
            }
            /* Start the task's new operation */
            var service = await ServicesController.FetchServiceById(proConnectionSettings, j.ServiceId);
            if (service == null)
            {
                Console.WriteLine($"Could not find service {j.ServiceId} for job {j.JobId}");
                continue;
            }

            bool operationSuccessful = false;

            Console.WriteLine($"Fetching Users from {service.Name}");
            switch (service.Name)
            {
                /* Handles Rapid Identity operation */
                case "Rapid Identity":
                    /* Prepare Connection */
                    string? riUrl = configuration.GetValue<string>("Connections:RapidIdentity:ApiUrl");
                    string? riUser = configuration.GetValue<string>("Connections:RapidIdentity:Username");
                    string? riPassword = configuration.GetValue<string>("Connections:RapidIdentity:Password");
                    if (riUrl == null || riUser == null || riPassword == null)
                    {
                        Console.WriteLine("Unable to load connection settings for Rapid Identity");
                        return;
                    }

                    RapidIdentityConnectionSettings riConnectionSettings = new RapidIdentityConnectionSettings {
                      Url = riUrl,
                      Username = riUser,
                      Password = riPassword,
                    };

                    /* Perform Sync */
                    operationSuccessful = await SyncManager.SyncRapidIdentity(proConnectionSettings, riConnectionSettings);
                    break;

                /* Handles Google operation */
                case "Google Workspace":
                    operationSuccessful = await SyncManager.SyncGoogle(proConnectionSettings);
                    break;

                /* Handles One Roster operation */
                case "One Roster":
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

                    operationSuccessful = await SyncManager.SyncOneRoster(proConnectionSettings, oneRoster);
                    break;

                default:
                    Console.WriteLine($"No routine found for service {service.Name}");
                    break;
            }

            if (operationSuccessful)
            {
                Console.WriteLine("Operation succeeded!");
            }
            else
            {
                Console.WriteLine("Operation encountered an error!");
            }

            /* Handle the end of a task */
            currTask.EndTime = DateTime.Now;
            currTask.Active = false;
            currTask.Notes = operationSuccessful ? "Operation succeeded" : "Operation encountered an error";
            await TasksController.PutTask(proConnectionSettings, currTask);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    Console.WriteLine("All ready jobs complete");
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var configuration = builder.Configuration;

while (true) {
    RunAsync(configuration).GetAwaiter().GetResult();
    System.Threading.Thread.Sleep(1000 * 60); // Sleep for 1 minutes
}
