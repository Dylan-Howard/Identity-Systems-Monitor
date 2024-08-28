using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prognosis_backend.Controllers;
using prognosis_backend.models;

namespace prognosis_backend
{
    class SyncManager
    {
        static readonly int _progressIncrement = 500;
        public static async Task<bool> SyncRapidIdentity(PrognosisConnectionSettings proSettings, RapidIdentityConnectionSettings riSettings)
        {
            Service? riService = await ServicesController.FetchServiceByName(proSettings, "Rapid Identity", 3);
            if (riService == null) {
                return false;
            }
            var riController = new RapidIdentityController(riSettings);
            List<RapidIdentityUser> users = await riController.GetUsersAsync();

            int totalRecords = 0;   
            foreach (RapidIdentityUser user in users)
            {
                /* Log progress */
                totalRecords += 1;
                if (totalRecords % _progressIncrement == 0)
                {
                    Console.WriteLine($"Processing {totalRecords} of {users.Count}");
                }

                if (user == null || user.Email == null) {
                    continue;
                }

                string? identifier = user.EmployeeType == "Staff" ? user.EmployeeId.ToString() : user.StudentId.ToString();
                if (identifier == null) {
                    continue;
                }

                Profile? match = await ProfilesController.FetchProfileByIdentifier(proSettings, identifier);

                /* Check for existing profile */
                if (match == null) {
                    Profile? toAdd = user;

                    if (toAdd == null) {
                        continue;
                    }

                    bool addSuccess = await ProfilesController.AddProfileRecord(proSettings, toAdd, 3);

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
                    bool updateSuccess = await ProfilesController.UpdateProfileRecord(proSettings, user, 3);

                    if (!updateSuccess) {
                        return false;
                    }
                }
            }

            Console.WriteLine($"Received {users.Count} user(s) from Rapid Identity");
            bool logRISuccessful = await TotalsController.LogSyncResults(proSettings, riService.ServiceId, users.Count, 3);

            if (logRISuccessful) {
              Console.WriteLine("Log succeeded!");
            } else {
              Console.WriteLine("Log encountered an error!");
            }

            return true;
        }
        public static async Task<bool> SyncGoogle(PrognosisConnectionSettings settings)
        {
            Service? googleService = await ServicesController.FetchServiceByName(settings, "Google Workspace", 3);
            if (googleService == null) {
                return false;
            }

            string googleServiceString = googleService.ServiceId.ToString();
            List<Link> googleLinks = GoogleController.FetchLinks(googleServiceString);

            List<Link> links = await LinkController.FetchLinkRecords(settings, 3);
            List<Profile> profiles = await ProfilesController.FetchProfileRecords(settings, 3);

            int totalRecords = 0;
            foreach (Link l in googleLinks)
            {
                /* Log progress */
                totalRecords += 1;
                if (totalRecords % _progressIncrement == 0)
                {
                    Console.WriteLine($"Processing {totalRecords} of {googleLinks.Count}");
                }

                /* Process linked account */
                Profile? profile = profiles.Find(
                    (p) => string.Equals(p.Email.ToLower(), l.ServiceIdentifier.ToLower()));
                
                if (profile == null)
                {
                  continue;
                }
                l.ProfileId = profile.ProfileId;

                Link? match = await LinkController.FetchLinkByServiceIdentifier(
                    settings,
                    l.ProfileId,
                    l.ServiceId,
                    l.ServiceIdentifier);

                if (match == null)
                {
                    bool addSuccess = await LinkController.AddLinkRecord(settings, l, 3);
                    if (!addSuccess)
                    {
                        Console.WriteLine($"Failed to add {l.ServiceIdentifier} as linked account.");
                        Console.WriteLine(l);
                        return false;
                    }
                }
                else
                {
                    l.ProfileId = match.ProfileId;
                    bool updateSuccess = await LinkController.UpdateLinkRecord(settings, l, 3);
                    if (!updateSuccess)
                    {
                        Console.WriteLine($"Failed to update {l.ServiceIdentifier} as linked account");
                        return false;
                    }
                }
            }

            // Log Totals
            Console.WriteLine($"Found {googleLinks.Count} link(s)");
            bool logGoogleSuccessful = await TotalsController.LogSyncResults(settings, googleService.ServiceId, googleLinks.Count, 3);

            if (!logGoogleSuccessful) {
              Console.WriteLine("Encountered an error when logging Google users!");
              return false;
            }

            return true;
        }
        public static async Task<bool> SyncOneRosterUsers(PrognosisConnectionSettings settings, OneRosterController oneRosterConnection)
        {
            Service? oneRosterService = await ServicesController.FetchServiceByName(settings, "One Roster", 3);
            if (oneRosterService == null) {
                return false;
            }

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

            List<Link> links = await LinkController.FetchLinkRecords(settings, 3);
            List<Profile> profiles = await ProfilesController.FetchProfileRecords(settings, 3);

            Console.WriteLine("Processing One Roster users");

            int totalLinkRecords = 0;
            foreach (Link l in oneRosterLinks)
            {
                /* Log progress */
                totalLinkRecords += 1;
                if (totalLinkRecords % _progressIncrement == 0)
                {
                    Console.WriteLine($"Processing {totalLinkRecords} of {oneRosterLinks.Count}");
                }

                /* Match Link to Profile */
                // @TODO: create FetchProfileByEmail function
                Profile? profile = profiles.Find(
                    (p) => string.Equals(p.Email.ToLower(), l.Email?.ToLower()));
                
                /* Check for existence of corresponding profile */
                if (profile == null)
                {
                  continue;
                }
                l.ProfileId = profile.ProfileId;

                /* Checks for existing linked account for this service */
                Link? match = await LinkController.FetchLinkByServiceIdentifier(
                    settings,
                    l.ProfileId,
                    oneRosterService.ServiceId,
                    l.ServiceIdentifier);

                /* Adds if linked account does not exist  */
                if (match == null)
                {
                    bool addSuccess = await LinkController.AddLinkRecord(settings, l, 3);
                    if (!addSuccess)
                    {
                        Console.WriteLine($"Failed to add {l.ServiceIdentifier} as a linked account.");
                        return false;
                    }
                }
                /* Updates linked account if exists */
                else
                {
                    l.ProfileId = match.ProfileId;
                    bool updateSuccess = await LinkController.UpdateLinkRecord(settings, l, 3);
                    if (!updateSuccess)
                    {
                        Console.WriteLine($"Failed to update {l.LinkId} as a linked account.");
                        return false;
                    }
                }
            }

            /* Log Totals */
            Console.WriteLine($"Found {oneRosterLinks.Count} link(s)");
            bool logOneRosterSuccessful = await TotalsController.LogSyncResults(settings, oneRosterService.ServiceId, oneRosterLinks.Count, 3);

            if (!logOneRosterSuccessful) {
              Console.WriteLine("Encountered an error when logging OneRoster users!");
              return false;
            }

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

            List<Class> classes = await ClassController.FetchClassRecords(settings);
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
            var totalClassRecords = 0;

            foreach (OneRosterClass c in oneRosterClasses)
            {
                /* Log progress */
                totalClassRecords += 1;
                if (totalClassRecords % _progressIncrement == 0)
                {
                    Console.WriteLine($"Processing {totalClassRecords} of {oneRosterClasses.Count} classes");
                }

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
                    bool addSuccess = await ClassController.AddClassRecord(settings, classRecord);

                    if (!addSuccess)
                    {
                        Console.WriteLine($"Failed to add {classRecord.Identifier} to orgs.");
                        return false;
                    }
                }
                else if (classStatus == 1)
                {
                    classMap[c.SourcedId] = 0;

                    bool updateSuccess = await ClassController.UpdateClassRecord(settings, classRecord);
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

                    bool updateSuccess = await ClassController.UpdateClassRecord(settings, c);
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
            Console.WriteLine($"Received {oneRosterEnrollments.Count} enrollments.");

            /* Setup class map */
            List<Class> classes = await ClassController.FetchClassRecords(settings);
            Console.WriteLine($"Referencing {classes.Count} current classes.");

            var classMap = new Dictionary<string, Guid>();
            foreach (Class c in classes)
            {
                classMap.Add(c.Identifier, c.SourcedId);
            }

            /* Setup user map */
            Service? oneRosterService = await ServicesController.FetchServiceByName(settings, "One Roster", 3);
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
            List<Enrollment> enrollments = await EnrollmentController.FetchEnrollmentRecords(settings);
            Console.WriteLine($"Referencing {enrollments.Count} current enrollments.");

            var enrollmentMap = new Dictionary<string, int>();
            foreach (Enrollment e in enrollments)
            {
                enrollmentMap.Add(e.Identifier, 1);
            }

            /* Process OneRosterOrg records */
            int totalEnrollmentRecords = 0;

            foreach (OneRosterEnrollment e in oneRosterEnrollments)
            {
                /* Log progress */
                totalEnrollmentRecords += 1;
                if (totalEnrollmentRecords % _progressIncrement == 0)
                {
                    Console.WriteLine($"Processing {totalEnrollmentRecords} of {oneRosterEnrollments.Count} enrollments");
                }

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

                if (classSourcedId == Guid.Empty)
                {
                    Console.WriteLine("Failed to match enrollment to a class.");
                    Console.WriteLine(e.Class.SourcedId);

                    continue;
                }

                Guid userSourcedId;
                userMap.TryGetValue(enrollmentRecord.UserSourcedId, out userSourcedId);
                enrollmentRecord.UserSourcedId = userSourcedId;

                if (userSourcedId == Guid.Empty)
                {
                    Console.WriteLine("Failed to match enrollment to a linked user account.");
                    Console.WriteLine(e.User.SourcedId);

                    continue;
                }

                int enrollmentStatus = 0;
                enrollmentMap.TryGetValue(enrollmentRecord.Identifier, out enrollmentStatus);
                if (enrollmentStatus == 0)
                {
                    bool addSuccess = await EnrollmentController.AddEnrollmentRecord(settings, enrollmentRecord);

                    if (!addSuccess)
                    {
                        Console.WriteLine($"Failed to add {enrollmentRecord.Identifier} to enrollments.");
                        return false;
                    }
                }
                else if (enrollmentStatus == 1)
                {
                    enrollmentMap[e.SourcedId] = 0;

                    bool updateSuccess = await EnrollmentController.UpdateEnrollmentRecord(settings, enrollmentRecord);
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

                    bool updateSuccess = await EnrollmentController.UpdateEnrollmentRecord(settings, e);
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
            Console.WriteLine("Processing OneRoster data");

            bool userSyncSuccess = await SyncOneRosterUsers(settings, oneRosterConnection);
            if (!userSyncSuccess)
            {
                Console.WriteLine("Failed to sync users");
                return false;
            }

            bool orgSyncSuccess = await SyncOneRosterOrgs(settings, oneRosterConnection);
            if (!orgSyncSuccess)
            {
                Console.WriteLine("Failed to sync orgs");
                return false;
            }

            bool classSyncSuccess = await SyncOneRosterClasses(settings, oneRosterConnection);
            if (!classSyncSuccess)
            {
                Console.WriteLine("Failed to sync classes");
                return false;
            }

            bool enrollmentSyncSuccess = await SyncOneRosterEnrollments(settings, oneRosterConnection);
            if (!enrollmentSyncSuccess)
            {
                Console.WriteLine("Failed to sync enrollments");
                return false;
            }

            Console.WriteLine("OneRoster sync succeeded!");
            return true;
        }
    }
}

