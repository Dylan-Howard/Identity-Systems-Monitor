using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prognosis_backend.models;

namespace prognosis_backend
{
    class SyncManager
    {
        static async Task<Service?> FetchServiceByName(PrognosisConnectionSettings settings, string serviceName, int retryCount)
        {
            if (retryCount == 0) {
              return null;
            }

            try
            {
              var db = new PrognosisContext(settings);

              Service targetService = db.Services.Single((srv) => srv.Name == serviceName);

              return targetService;
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    return await FetchServiceByName(settings, serviceName, retryCount - 1);
                }

                Console.WriteLine(e.ToString());
                return null;
            }
        }
        
        public static async Task<List<Profile>> FetchProfileRecords(PrognosisConnectionSettings settings, int retryCount)
        {
            List<Profile> profiles = [];
            if (retryCount == 0) {
                return profiles;
            }

            try
            {
                var db = new PrognosisContext(settings);
                profiles = await db.Profiles.ToListAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    Console.WriteLine("Connection timed out. Retrying...");
                    return await FetchProfileRecords(settings, retryCount - 1);
                } else {
                    Console.WriteLine(e.ToString());
                }
            }
            
            return profiles;
        }

        public static async Task<List<Link>> FetchLinkRecords(PrognosisConnectionSettings settings, int retryCount)
        {
            List<Link> links = [];
            if (retryCount == 0) {
                return links;
            }

            try
            {
                var db = new PrognosisContext(settings);
                links = await db.Links.ToListAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    Console.WriteLine("Connection timed out. Retrying...");
                    return await FetchLinkRecords(settings, retryCount - 1);
                } else {
                    Console.WriteLine(e.ToString());
                }
            }
            
            return links;
        }

        static async Task<bool> AddProfileRecord(PrognosisConnectionSettings settings, Profile addProfile, int retryCount)
        {
            if (retryCount == 0) {
              return false;
            }

            try
            {
              var db = new PrognosisContext(settings);
              Console.WriteLine("Inserting a new profile");

              Console.WriteLine(addProfile);

              await db.AddAsync(addProfile);
              await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                  Console.WriteLine("Connection timed out. Retrying...");
                    return await AddProfileRecord(settings, addProfile, retryCount - 1);
                }

                Console.WriteLine(addProfile);

                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        static async Task<bool> UpdateProfileRecord(PrognosisConnectionSettings settings, Profile updateProfile, int retryCount)
        {
            if (retryCount == 0) {
              return false;
            }

            try
            {
              var db = new PrognosisContext(settings);
              Console.WriteLine(updateProfile);

              Profile p = db.Profiles.Single((prf) => prf.Identifier == updateProfile.Identifier);

              Console.WriteLine("Found match!");
              Console.WriteLine(p);

              if (p.FirstName != updateProfile.FirstName) {
                p.FirstName = updateProfile.FirstName;
              }
              if (p.MiddleName != updateProfile.MiddleName) {
                p.MiddleName = updateProfile.MiddleName;
              }
              if (p.LastName != updateProfile.LastName) {
                p.LastName = updateProfile.LastName;
              }
              if (p.Email != updateProfile.Email) {
                p.Email = updateProfile.Email;
              }
              if (p.Status != updateProfile.Status) {
                p.Status = updateProfile.Status;
              }
              if (p.Locked != updateProfile.Locked) {
                p.Locked = updateProfile.Locked;
              }
              if (p.Claimed != updateProfile.Claimed) {
                p.Claimed = updateProfile.Claimed;
              }
              if (p.MfaMethod != updateProfile.MfaMethod) {
                p.MfaMethod = updateProfile.MfaMethod;
              }
              
              await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    Console.WriteLine("Connection timed out. Retrying...");
                    return await UpdateProfileRecord(settings, updateProfile, retryCount - 1);
                }
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        static async Task<bool> AddLinkRecord(PrognosisConnectionSettings settings, Link addLink, int retryCount)
        {
            if (retryCount == 0) {
              return false;
            }

            try
            {
              var db = new PrognosisContext(settings);
              Console.WriteLine("Inserting a new link");

              Console.WriteLine(addLink);

              await db.AddAsync(addLink);
              await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    Console.WriteLine("Connection timed out. Retrying...");
                    return await AddLinkRecord(settings, addLink, retryCount - 1);
                }

                Console.WriteLine(e.ToString());
                Console.WriteLine(e.Number);
                return false;
            }

            return true;
        }

        static async Task<bool> UpdateLinkRecord(PrognosisConnectionSettings settings, Link updateLink, Guid serviceId, int retryCount)
        {
            if (retryCount == 0) {
              return false;
            }

            try
            {
              var db = new PrognosisContext(settings);
              Console.WriteLine("Updating a link");

              Link l = db.Links.Single(
                (l) => Equals(l.ProfileId, updateLink.ProfileId)
                        && serviceId.ToString() == updateLink.ServiceId.ToString());

              if (l.FirstName != updateLink.FirstName) {
                l.FirstName = updateLink.FirstName;
              }
              if (l.LastName != updateLink.LastName) {
                l.LastName = updateLink.LastName;
              }
              if (l.Active != updateLink.Active) {
                l.Active = updateLink.Active;
              }
              if (l.OrgUnitPath != updateLink.OrgUnitPath) {
                l.OrgUnitPath = updateLink.OrgUnitPath;
              }
              if (l.Organization != updateLink.Organization) {
                l.Organization = updateLink.Organization;
              }
              if (l.PhotoUrl != updateLink.PhotoUrl) {
                l.PhotoUrl = updateLink.PhotoUrl;
              }
              if (l.Address != updateLink.Address) {
                l.Address = updateLink.Address;
              }
              if (l.Phone != updateLink.Phone) {
                l.Phone = updateLink.Phone;
              }
              if (l.CreatedDate != updateLink.CreatedDate) {
                l.CreatedDate = updateLink.CreatedDate;
              }
              if (l.LastActivity != updateLink.LastActivity) {
                l.LastActivity = updateLink.LastActivity;
              }
              
              await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    Console.WriteLine("Connection timed out. Retrying...");
                    return await UpdateLinkRecord(settings, updateLink, serviceId, retryCount - 1);
                }
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        static async Task<bool> LogSyncResults(PrognosisConnectionSettings settings, string service, int total, int retryCount)
        {
            if (retryCount == 0) {
              return false;
            }

            try
            {
              var db = new PrognosisContext(settings);
              Console.WriteLine("Inserting a new service total");

              Service? targetService = await FetchServiceByName(settings, service, retryCount);

              if (targetService == null) {
                return false;
              }

              Total toAdd = new Total {
                ServiceId = targetService.ServiceId,
                Count = total,
                Timestamp = DateTime.Now,
              };

              await db.AddAsync(toAdd);
              await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    return await LogSyncResults(settings, service, total, retryCount - 1);
                }

                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        public static async Task<bool> SyncRapidIdentity(PrognosisConnectionSettings proSettings, RapidIdentityConnectionSettings riSettings, List<Profile> profiles)
        {
            List<RapidIdentityUser> users = await RapidIdentity.GetUsersAsync(riSettings);

            Console.WriteLine(users.Count);
            
            foreach (RapidIdentityUser user in users)
            {
                if (user == null || user.Email == null) {
                  continue;
                }

                int? identifier = user.EmployeeType == "Staff" ? user.EmployeeId : user.StudentId;
                if (identifier == null) {
                  continue;
                }

                Profile? match = profiles.Find((p) => p.Identifier == identifier.ToString());

                /* Check for existing profile */
                if (match == null) {
                    Console.WriteLine($"Adding {user.Email}");
                    Console.WriteLine(user);
                    Profile? toAdd = user;
                    Console.WriteLine(toAdd);

                    if (toAdd == null) {
                      continue;
                    }

                    bool addSuccess = await AddProfileRecord(proSettings, toAdd, 3);

                      if (!addSuccess) {
                        return false;
                      }
                    
                    continue;
                }

                /* Check for profile changes */
                if (
                    match.FirstName != user.FirstName
                    || match.MiddleName != user.MiddleName
                    || match.LastName != user.LastName
                    || match.Email != user.Email
                    || (!!match.Status && user.IsActive == "inactive")
                    || match.Claimed != user.ClaimFlag
                    || match.Locked != user.IsLocked
                    || match.MfaMethod != user.MfaMethod
                  )
                {
                    Console.WriteLine($"Updating {user.Email}");
                    bool updateSuccess = await UpdateProfileRecord(proSettings, user, 3);

                    if (!updateSuccess) {
                      return false;
                    }
                }
            }

            Console.WriteLine($"Received {users.Count} user(s) from Rapid Identity");
            bool logRISuccessful = await LogSyncResults(proSettings, "Rapid Idenity", users.Count, 3);

            if (logRISuccessful) {
              Console.WriteLine("Log succeeded!");
            } else {
              Console.WriteLine("Log encountered an error!");
            }

            return true;
        }

        public static async Task<bool> SyncGoogle(PrognosisConnectionSettings settings)
        {
            Service? googleService = await FetchServiceByName(settings, "Google Workspace", 3);
            if (googleService == null) {
                return false;
            }

            string googleServiceString = googleService.ServiceId.ToString();
            List<Link> googleLinks = GoogleWorkspace.FetchLinks(googleServiceString);

            List<Link> links = await FetchLinkRecords(settings, 3);
            List<Profile> profiles = await FetchProfileRecords(settings, 3);

            Console.WriteLine("Processing Google data");

            foreach (Link l in googleLinks)
            {
                Profile? profile = profiles.Find(
                    (p) => string.Equals(p.Email.ToLower(), l.ServiceIdentifier.ToLower()));
                
                if (profile == null)
                {
                  continue;
                }
                l.ProfileId = profile.ProfileId;

                Link? match = links.Find(
                    (lnk) => lnk.ProfileId == l.ProfileId && lnk.ServiceId.ToString() == googleServiceString);

                if (match == null)
                {
                    bool addSuccess = await AddLinkRecord(settings, l, 3);
                    if (!addSuccess)
                    {
                        return false;
                    }
                }
                else
                {
                    l.ProfileId = match.ProfileId;
                    bool updateSuccess = await UpdateLinkRecord(settings, l, googleService.ServiceId, 3);
                    if (!updateSuccess)
                    {
                        return false;
                    }
                }
            }

            // Log Totals
            Console.WriteLine($"Found {googleLinks.Count} link(s)");
            bool logGoogleSuccessful = await LogSyncResults(settings, "Google", googleLinks.Count, 3);

            if (!logGoogleSuccessful) {
              Console.WriteLine("Log encountered an error!");
              return false;
            }

            Console.WriteLine("Google sync succeeded!");
            return true;
        }
    
    }
}