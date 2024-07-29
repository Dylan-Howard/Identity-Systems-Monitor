using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prognosis_backend.Controllers;
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
                return false;
            }

            return true;
        }

        static LinkRecordChanges HasLinkRecordChanged(Link link1, Link link2)
        {
            List<string> changedFields = [];

            if (link1.FirstName != link2.FirstName) {
                link1.FirstName = link2.FirstName;
                changedFields.Add("FirstName");
            }
              if (link1.LastName != link2.LastName) {
                link1.LastName = link2.LastName;
                changedFields.Add("LastName");
            }
              if (link1.Active != link2.Active) {
                link1.Active = link2.Active;
                changedFields.Add("Active");
            }
              if (link1.OrgUnitPath != link2.OrgUnitPath) {
                link1.OrgUnitPath = link2.OrgUnitPath;
                changedFields.Add("OrgUnitPath");
            }
            if (link1.Organization != link2.Organization) {
                link1.Organization = link2.Organization;
                changedFields.Add("Organization");
            }
            if (link1.PhotoUrl != link2.PhotoUrl) {
                link1.PhotoUrl = link2.PhotoUrl;
                changedFields.Add("PhotoUrl");
            }
            if (link1.Email != link2.Email) {
                link1.Email = link2.Email;
                changedFields.Add("Email");
            }
            if (link1.Address != link2.Address) {
                link1.Address = link2.Address;
                changedFields.Add("Address");
            }
            if (link1.Phone != link2.Phone) {
                link1.Phone = link2.Phone;
                changedFields.Add("Phone");
            }
            if (link1.CreatedDate != link2.CreatedDate) {
                link1.CreatedDate = link2.CreatedDate;
                changedFields.Add("CreatedDate");
            }
            if (link1.LastActivity != link2.LastActivity) {
                link1.LastActivity = link2.LastActivity;
                changedFields.Add("LastActivity");
            }
            
            return new LinkRecordChanges {
                ChangedFields = changedFields
            };
        }

        static async Task<bool> UpdateLinkRecord(PrognosisConnectionSettings settings, Link updateLink, int retryCount)
        {
            if (retryCount == 0) {
                return false;
            }

            try
            {
                var db = new PrognosisContext(settings);

                Link? link = await db.Links.FirstOrDefaultAsync(
                  (l) => Equals(l.ProfileId, updateLink.ProfileId)
                          && l.ServiceId.ToString() == updateLink.ServiceId.ToString()
                          && l.ServiceIdentifier == updateLink.ServiceIdentifier);

                if (link == null)
                {
                    return false;
                }

                LinkRecordChanges changes = HasLinkRecordChanged(updateLink, link);

                if (changes.ChangedFields.Count == 0)
                {
                    return true;
                }

                Console.WriteLine("Updating a link");
                
                await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    Console.WriteLine("Connection timed out. Retrying...");
                    return await UpdateLinkRecord(settings, updateLink, retryCount - 1);
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
                    Profile? toAdd = user;

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
                    bool updateSuccess = await UpdateLinkRecord(settings, l, 3);
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
        public static async Task<bool> SyncOneRoster(PrognosisConnectionSettings settings, OneRosterController oneRosterConnection)
        {
            Service? oneRosterService = await FetchServiceByName(settings, "One Roster", 3);
            if (oneRosterService == null) {
                return false;
            }

            // List<OneRosterOrg> oneRosterOrgs = await oneRosterConnection.FetchOneRosterOrgsAsync();
            // Console.WriteLine($"Received {oneRosterOrgs.Count} orgs.");

            // List<OneRosterClass> oneRosterClasses = await oneRoster.FetchOneRosterClassesAsync();
            // Console.WriteLine($"Received {oneRosterClasses.Count} classes.");
            
            // List<OneRosterEnrollment> oneRosterEnrollments = await oneRoster.FetchOneRosterEnrollmentsAsync();
            // Console.WriteLine($"Received {oneRosterEnrollments.Count} enrollments.");

            string oneRosterServiceString = oneRosterService.ServiceId.ToString();
            List<OneRosterUser> oneRosterUsers = await oneRosterConnection.FetchOneRosterUsersAsync();
            Console.WriteLine($"Received {oneRosterUsers.Count} users.");
            List<Link> oneRosterLinks = oneRosterUsers.Select((usr) => new Link {
                ServiceId = oneRosterService.ServiceId,
                ServiceIdentifier = usr.SourcedId.ToString(),
                Active = usr.EnabledUser == "active",
                FirstName = usr.GivenName ?? "",
                LastName = usr.FamilyName ?? "",
                Email = usr.Email,
                Phone = usr.SMS,
                OrgUnitPath = "None",
                LastActivity = usr.DateLastModified,
            }).ToList();

            List<Link> links = await FetchLinkRecords(settings, 3);
            List<Profile> profiles = await FetchProfileRecords(settings, 3);

            Console.WriteLine("Processing One Roster data");

            // foreach (Link l in oneRosterLinks)
            // {
            //     /* Match Link to Profile */
            //     Profile? profile = profiles.Find(
            //         (p) => string.Equals(p.Email.ToLower(), l.Email?.ToLower()));
                
            //     /* Check for existence of corresponding profile */
            //     if (profile == null)
            //     {
            //       continue;
            //     }
            //     l.ProfileId = profile.ProfileId;

            //     /* Checks for existing linked account for this service */
            //     Link? match = links.Find(
            //         (lnk) => lnk.ProfileId == l.ProfileId && lnk.ServiceId.ToString() == oneRosterServiceString);

            //     /* Adds if linked account does not exist  */
            //     if (match == null)
            //     {
            //         bool addSuccess = await AddLinkRecord(settings, l, 3);
            //         if (!addSuccess)
            //         {
            //             return false;
            //         }
            //     }
            //     /* Updates linked account if exists */
            //     else
            //     {
            //         l.ProfileId = match.ProfileId;
            //         bool updateSuccess = await UpdateLinkRecord(settings, l, 3);
            //         if (!updateSuccess)
            //         {
            //             return false;
            //         }
            //     }
            // }

            /* Log Totals */
            Console.WriteLine($"Found {oneRosterLinks.Count} link(s)");
            bool logOneRosterSuccessful = await LogSyncResults(settings, "One Roster", oneRosterLinks.Count, 3);

            if (!logOneRosterSuccessful) {
              Console.WriteLine("Log encountered an error!");
              return false;
            }

            Console.WriteLine("One Roster sync succeeded!");
            return true;
        }
    
    }

    class LinkRecordChanges
    {
        public required IList<string> ChangedFields { get; set; }
    }
}

