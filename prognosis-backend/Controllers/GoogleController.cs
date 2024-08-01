
using Google.Apis.Auth.OAuth2;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using prognosis_backend.models;
using Google.Apis.AlertCenter.v1beta1.Data;
using Google.Apis.AlertCenter.v1beta1;

namespace prognosis_backend
{
    // Class to demonstrate the use of Directory users list API
	class GoogleController
  {
      static readonly int _progressIncrement = 500;
        /* Global instance of the scopes required by this quickstart.
         If modifying these scopes, delete your previously saved token.json/ folder. */
        static string[] Scopes = { DirectoryService.Scope.AdminDirectoryUserReadonly };
        static string ApplicationName = "Directory API .NET Quickstart";

        public static void ListUsers()
        {
            try
            {
                UserCredential credential;
                // Load client secrets.
                using (var stream =
                       new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    /* The file token.json stores the user's access and refresh tokens, and is created
                     automatically when the authorization flow completes for the first time. */
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Directory API service.
                var service = new DirectoryService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Define parameters of request.
                UsersResource.ListRequest request = service.Users.List();
                request.Customer = "my_customer";
                request.MaxResults = 10;
                request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;

                // List users.
                IList<Google.Apis.Admin.Directory.directory_v1.Data.User> users = request.Execute().UsersValue;
                Console.WriteLine("Users:");
                if (users == null || users.Count == 0)
                {
                    Console.WriteLine("No users found.");
                    return;
                }
                foreach (var userItem in users)
                {
                    Console.WriteLine("{0} ({1})", userItem.PrimaryEmail,
                        userItem.Name.FullName);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static IList<Google.Apis.Admin.Directory.directory_v1.Data.User> FetchUsers()
        {
            IList<Google.Apis.Admin.Directory.directory_v1.Data.User> users = new List<Google.Apis.Admin.Directory.directory_v1.Data.User>();

            try
            {
                UserCredential credential;
                // Load client secrets.
                using (var stream =
                       new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    /* The file token.json stores the user's access and refresh tokens, and is created
                     automatically when the authorization flow completes for the first time. */
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Directory API service.
                var service = new DirectoryService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Define parameters of request.
                UsersResource.ListRequest request = service.Users.List();
                request.Customer = "my_customer";
                request.MaxResults = 100;
                request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;

                // List users.
                do
                {
                    var res = request.Execute();
                    users = users.Concat(res.UsersValue).ToList();

                    request.PageToken = res.NextPageToken;

                    if (users.Count % _progressIncrement == 0)
                    {
                        Console.WriteLine($"Fetched {res.UsersValue.Count} users. {users.Count} total now.");
                    }
                }
                while (request.PageToken != null);
                // while (request.PageToken != null && users.Count < 199);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            return users;
        }
        public static List<Link> FetchLinks(string serviceId)
        {
            IList<Google.Apis.Admin.Directory.directory_v1.Data.User> users = FetchUsers();
            List<Link> links = [];

            foreach (Google.Apis.Admin.Directory.directory_v1.Data.User u in users)
            {
                string[] addresses = [];
                if (u.Addresses != null)
                {
                    foreach (var a in u.Addresses)
                    {
                        addresses.Append($"{a.StreetAddress} {a.Locality}, {a.Region} {a.PostalCode}");
                    }
                }

                string? address = u.Addresses != null ? string.Join(";", addresses) : "";
                string? phone = u.Phones != null ? string.Join(";", u.Phones.FirstOrDefault((p) => p.Primary == true)?.Value) : null;
                string? photoUrl = u.ThumbnailPhotoUrl != null ? u.ThumbnailPhotoUrl.ToString() : "";

                links.Add(new Link {
                    ServiceId = new Guid(serviceId),
                    ServiceIdentifier = u.PrimaryEmail,
                    Active = u.Suspended != true,
                    FirstName = u.Name.GivenName,
                    LastName = u.Name.FamilyName,
                    Email = u.PrimaryEmail,
                    Address = string.IsNullOrEmpty(address) ? "" : address,
                    Phone = string.IsNullOrEmpty(phone) ? "" : phone,
                    PhotoUrl = string.IsNullOrEmpty(photoUrl) ? "" : photoUrl,
                    OrgUnitPath = u.OrgUnitPath,
                    Organization = u.Locations != null ? u.Locations.ToString() : "",
                    CreatedDate = DateTime.Parse(u.CreationTimeRaw.ToString()),
                    LastActivity = DateTime.Parse(u.LastLoginTimeRaw.ToString())
                });
            }

            return links;
        }
    }
}