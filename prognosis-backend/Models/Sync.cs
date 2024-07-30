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

        static RecordChanges HasLinkRecordChanged(Link link1, Link link2)
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
            
            return new RecordChanges {
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

                RecordChanges changes = HasLinkRecordChanged(updateLink, link);

                if (changes.ChangedFields.Count == 0)
                {
                    return true;
                }
                
                await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    Console.WriteLine("Connection timed out. Retrying...");
                    return await UpdateLinkRecord(settings, updateLink, retryCount - 1);
                }
                Console.WriteLine(e.ToString());
                Console.WriteLine(updateLink);
                return false;
            }

            return true;
        }

        static async Task<bool> LogSyncResults(PrognosisConnectionSettings settings, Guid serviceId, int total, int retryCount)
        {
            if (retryCount == 0) {
                return false;
            }

            try
            {
                var db = new PrognosisContext(settings);

                Total toAdd = new Total {
                  ServiceId = serviceId,
                  Count = total,
                  Timestamp = DateTime.Now,
                };

                await db.AddAsync(toAdd);
                await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {
                if (e.Number == -2) {
                    return await LogSyncResults(settings, serviceId, total, retryCount - 1);
                }

                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
        public static async Task<bool> SyncRapidIdentity(PrognosisConnectionSettings proSettings, RapidIdentityConnectionSettings riSettings, List<Profile> profiles)
        {
            Service? riService = await FetchServiceByName(proSettings, "Rapid Identity", 3);
            if (riService == null) {
                return false;
            }
            List<RapidIdentityUser> users = await RapidIdentityController.GetUsersAsync(riSettings);
            
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
            bool logRISuccessful = await LogSyncResults(proSettings, riService.ServiceId, users.Count, 3);

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
            List<Link> googleLinks = GoogleController.FetchLinks(googleServiceString);

            List<Link> links = await FetchLinkRecords(settings, 3);
            List<Profile> profiles = await FetchProfileRecords(settings, 3);

            foreach (Link l in googleLinks)
            {
                Console.WriteLine(l.Email);
                Profile? profile = profiles.Find(
                    (p) => string.Equals(p.Email.ToLower(), l.ServiceIdentifier.ToLower()));
                
                if (profile == null)
                {
                  continue;
                }
                l.ProfileId = profile.ProfileId;

                Link? match = links.Find(
                    (lnk) => lnk.ProfileId == l.ProfileId
                      && lnk.ServiceId.ToString() == googleServiceString
                      && lnk.ServiceIdentifier == l.ServiceIdentifier);

                if (match == null)
                {
                    bool addSuccess = await AddLinkRecord(settings, l, 3);
                    if (!addSuccess)
                    {
                        Console.WriteLine("Failed to add");
                        Console.WriteLine(l);
                        return false;
                    }
                }
                else
                {
                    l.ProfileId = match.ProfileId;
                    bool updateSuccess = await UpdateLinkRecord(settings, l, 3);
                    if (!updateSuccess)
                    {
                        Console.WriteLine("Failed to update");
                        Console.WriteLine(l);
                        return false;
                    }
                }
            }

            // Log Totals
            Console.WriteLine($"Found {googleLinks.Count} link(s)");
            bool logGoogleSuccessful = await LogSyncResults(settings, googleService.ServiceId, googleLinks.Count, 3);

            if (!logGoogleSuccessful) {
              Console.WriteLine("Log encountered an error!");
              return false;
            }

            Console.WriteLine("Google sync succeeded!");
            return true;
        }
        public static async Task<bool> SyncOneRosterOrgs(PrognosisConnectionSettings settings, OneRosterController oneRosterConnection)
        {
            List<OneRosterOrg> oneRosterOrgs = await oneRosterConnection.FetchOneRosterOrgsAsync();
            Console.WriteLine($"Received {oneRosterOrgs.Count} orgs.");

            List<Org> orgs = await OrgController.FetchOrgRecords(settings, 3);
            Console.WriteLine($"Referencing {orgs.Count} current orgs.");
            
            /* Setup org map */
            var classMap = new Dictionary<string, int>();
            foreach (Org o in orgs)
            {
                classMap.Add(o.Identifier, 1);
            }

            /* Process OneRosterOrg records */
            foreach (OneRosterOrg o in oneRosterOrgs)
            {
                Org? orgRecord = o;

                if (orgRecord == null)
                {
                    continue;
                }

                int orgStatus = 0;
                classMap.TryGetValue(orgRecord.Identifier, out orgStatus);
                if (orgStatus == 0)
                {
                    bool addSuccess = await OrgController.AddOrgRecord(settings, orgRecord, 3);

                    if (addSuccess)
                    {
                        Console.WriteLine($"{orgRecord.Identifier} successfully added to orgs.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to add {orgRecord.Identifier} to orgs.");
                        return false;
                    }
                }
                else if (orgStatus == 1)
                {
                    classMap[o.Identifier] = 0;

                    bool updateSuccess = await OrgController.UpdateOrgRecord(settings, orgRecord, 3);
                    if (updateSuccess)
                    {
                        Console.WriteLine($"{orgRecord.Identifier} successfully updated.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to update {orgRecord.Identifier}.");
                        return false;
                    }
                }
            }

            /* Set unmatched records to inactive */
            foreach (Org o in orgs)
            {
                if (classMap[o.Identifier] == 1)
                {
                    o.Status = false;

                    bool updateSuccess = await OrgController.UpdateOrgRecord(settings, o, 3);
                    if (updateSuccess)
                    {
                        Console.WriteLine($"{o.Identifier} successfully updated to orgs.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to update {o.Identifier} to orgs.");
                        return false;
                    }
                }
            }

            return true;
        }
        public static async Task<bool> SyncOneRosterClasses(PrognosisConnectionSettings settings, OneRosterController oneRosterConnection)
        {
            List<OneRosterClass> oneRosterClasses = await oneRosterConnection.FetchOneRosterClassesAsync();
            Console.WriteLine($"Received {oneRosterClasses.Count} classes.");

            List<Class> classes = await ClassController.FetchClassRecords(settings, 3);
            Console.WriteLine($"Referencing {classes.Count} current classes.");

            List<Org> orgs = await OrgController.FetchOrgRecords(settings, 3);
            Console.WriteLine($"Referencing {orgs.Count} current orgs.");
            
            /* Setup class map */
            var classMap = new Dictionary<string, int>();
            foreach (Class c in classes)
            {
                classMap.Add(c.Identifier, 1);
            }

            /* Setup org map */
            var orgMap = new Dictionary<Guid, Guid>();
            foreach (Org o in orgs)
            {
                orgMap.Add(Guid.Parse(o.Identifier), o.SourcedId);
            }

            /* Process OneRosterOrg records */
            foreach (OneRosterClass c in oneRosterClasses)
            {
                Class? classRecord = c;

                if (classRecord == null || classRecord.OrgSourcedId == Guid.Empty)
                {
                    continue;
                }

                Guid classOrg;
                orgMap.TryGetValue(classRecord.OrgSourcedId, out classOrg);
                classRecord.OrgSourcedId = classOrg;

                int classStatus = 0;
                classMap.TryGetValue(classRecord.Identifier, out classStatus);
                if (classStatus == 0)
                {
                    bool addSuccess = await ClassController.AddClassRecord(settings, classRecord, 3);

                    if (!addSuccess)
                    {
                        Console.WriteLine($"Failed to add {classRecord.Identifier} to orgs.");
                        return false;
                    }
                }
                else if (classStatus == 1)
                {
                    classMap[c.SourcedId] = 0;

                    bool updateSuccess = await ClassController.UpdateClassRecord(settings, classRecord, 3);
                    if (!updateSuccess)
                    {
                        Console.WriteLine($"Failed to update {classRecord.Identifier}.");
                        return false;
                    }
                }
            }

            /* Set unmatched records to inactive */
            foreach (Class c in classes)
            {
                if (classMap[c.Identifier] == 1)
                {
                    c.Status = false;

                    bool updateSuccess = await ClassController.UpdateClassRecord(settings, c, 3);
                    if (!updateSuccess)
                    {
                        Console.WriteLine($"Failed to update {c.Identifier}.");
                        return false;
                    }
                }
            }

            return true;
        }
        public static async Task<bool> SyncOneRosterEnrollments(PrognosisConnectionSettings settings, OneRosterController oneRosterConnection)
        {
            List<OneRosterEnrollment> oneRosterEnrollments = await oneRosterConnection.FetchOneRosterEnrollmentsAsync();
            Console.WriteLine($"Received {oneRosterEnrollments.Count} classes.");

            /* Setup class map */
            List<Class> classes = await ClassController.FetchClassRecords(settings, 3);
            Console.WriteLine($"Referencing {classes.Count} current classes.");

            var classMap = new Dictionary<string, Guid>();
            foreach (Class c in classes)
            {
                classMap.Add(c.Identifier, c.SourcedId);
            }

            /* Setup user map */
            Service? oneRosterService = await FetchServiceByName(settings, "One Roster", 3);
            if (oneRosterService == null)
            {
                return false;
            }

            List<Link> users = await LinkController.FetchLinkRecords(settings, 3);
            users = users.FindAll((u) => u.ServiceId == oneRosterService.ServiceId);
            Console.WriteLine($"Referencing {users.Count} current users.");

            var userMap = new Dictionary<Guid, Guid>();
            foreach (Link u in users)
            {
                userMap.Add(Guid.Parse(u.ServiceIdentifier), u.LinkId);
            }
            
            /* Setup enrollments map */
            List<Enrollment> enrollments = await EnrollmentController.FetchEnrollmentRecords(settings, 3);
            Console.WriteLine($"Referencing {enrollments.Count} current enrollments.");

            var enrollmentMap = new Dictionary<string, int>();
            foreach (Enrollment e in enrollments)
            {
                enrollmentMap.Add(e.Identifier, 1);
            }

            /* Process OneRosterOrg records */
            foreach (OneRosterEnrollment e in oneRosterEnrollments)
            {
                Enrollment? enrollmentRecord = e;

                if (enrollmentRecord == null
                    || enrollmentRecord.UserSourcedId == Guid.Empty
                    || string.IsNullOrEmpty(e.Class.SourcedId))
                {
                    Console.WriteLine("Failed to match enrollment to a class due to missing or no data.");
                    continue;
                }

                Guid classSourcedId;
                classMap.TryGetValue(e.Class.SourcedId, out classSourcedId);
                enrollmentRecord.ClassSourcedId = classSourcedId;

                Guid userSourcedId;
                userMap.TryGetValue(enrollmentRecord.UserSourcedId, out userSourcedId);
                enrollmentRecord.UserSourcedId = userSourcedId;

                if (userSourcedId == Guid.Empty)
                {
                    Console.WriteLine("Failed to match enrollment to a linked user account.");
                    Console.WriteLine(e.User.SourcedId);

                    return false;
                    continue;
                }

                int enrollmentStatus = 0;
                enrollmentMap.TryGetValue(enrollmentRecord.Identifier, out enrollmentStatus);
                if (enrollmentStatus == 0)
                {
                    bool addSuccess = await EnrollmentController.AddEnrollmentRecord(settings, enrollmentRecord, 3);

                    if (!addSuccess)
                    {
                        Console.WriteLine($"Failed to add {enrollmentRecord.Identifier} to enrollments.");
                        return false;
                    }
                }
                else if (enrollmentStatus == 1)
                {
                    enrollmentMap[e.SourcedId] = 0;

                    bool updateSuccess = await EnrollmentController.UpdateEnrollmentRecord(settings, enrollmentRecord, 3);
                    if (!updateSuccess)
                    {
                        Console.WriteLine($"Failed to update {enrollmentRecord.Identifier}.");
                        return false;
                    }
                }
            }

            /* Set unmatched records to inactive */
            foreach (Enrollment e in enrollments)
            {
                if (enrollmentMap[e.Identifier] == 1)
                {
                    e.Status = false;

                    bool updateSuccess = await EnrollmentController.UpdateEnrollmentRecord(settings, e, 3);
                    if (!updateSuccess)
                    {
                        Console.WriteLine($"Failed to update {e.Identifier}.");
                        return false;
                    }
                }
            }

            return true;
        }
        public static async Task<bool> SyncOneRoster(PrognosisConnectionSettings settings, OneRosterController oneRosterConnection)
        {
            Service? oneRosterService = await FetchServiceByName(settings, "One Roster", 3);
            if (oneRosterService == null) {
                return false;
            }

            // bool orgSyncSuccess = await SyncOneRosterOrgs(settings, oneRosterConnection);
            // if (!orgSyncSuccess)
            // {
            //     Console.WriteLine("Failed to sync orgs");
            //     return false;
            // }

            // bool classSyncSuccess = await SyncOneRosterClasses(settings, oneRosterConnection);
            // if (!classSyncSuccess)
            // {
            //     Console.WriteLine("Failed to sync classes");
            //     return false;
            // }

            bool enrollmentSyncSuccess = await SyncOneRosterEnrollments(settings, oneRosterConnection);
            if (!enrollmentSyncSuccess)
            {
                Console.WriteLine("Failed to sync enrollments");
                return false;
            }

            string oneRosterServiceString = oneRosterService.ServiceId.ToString();
            // List<OneRosterUser> oneRosterUsers = await oneRosterConnection.FetchOneRosterUsersAsync();
            // Console.WriteLine($"Received {oneRosterUsers.Count} users.");
            // List<Link> oneRosterLinks = oneRosterUsers.Select((usr) => new Link {
            //     ServiceId = oneRosterService.ServiceId,
            //     ServiceIdentifier = usr.SourcedId.ToString(),
            //     Active = usr.EnabledUser == "active",
            //     FirstName = usr.GivenName ?? "",
            //     LastName = usr.FamilyName ?? "",
            //     Email = usr.Email,
            //     Phone = usr.SMS,
            //     OrgUnitPath = "None",
            //     LastActivity = usr.DateLastModified,
            // }).ToList();

            // List<Link> links = await FetchLinkRecords(settings, 3);
            // List<Profile> profiles = await FetchProfileRecords(settings, 3);

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
            // Console.WriteLine($"Found {oneRosterLinks.Count} link(s)");
            // bool logOneRosterSuccessful = await LogSyncResults(settings, oneRosterService.ServiceId, oneRosterLinks.Count, 3);

            // if (!logOneRosterSuccessful) {
            //   Console.WriteLine("Log encountered an error!");
            //   return false;
            // }

            Console.WriteLine("One Roster sync succeeded!");
            return true;
        }
    }
}

