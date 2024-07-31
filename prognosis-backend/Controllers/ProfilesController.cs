using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prognosis_backend.models;

namespace prognosis_backend;

public static class ProfilesController
{
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
    public static async Task<Profile?> FetchProfileByIdentifier(PrognosisConnectionSettings settings, string indentifier)
    {
        try
        {
            var db = new PrognosisContext(settings);
            Profile? profile = await db.Profiles.FirstOrDefaultAsync(
                (p) => p.Identifier == indentifier);

            return profile;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }

        return null;
    }
    public static async Task<bool> AddProfileRecord(PrognosisConnectionSettings settings, Profile addProfile, int retryCount)
        {
            if (retryCount == 0) {
              return false;
            }

            try
            {
              var db = new PrognosisContext(settings);

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

        public static async Task<bool> UpdateProfileRecord(PrognosisConnectionSettings settings, Profile updateProfile, int retryCount)
        {
            if (retryCount == 0) {
              return false;
            }

            try
            {
              var db = new PrognosisContext(settings);

              Profile? p = await db.Profiles.FirstOrDefaultAsync((prf) => prf.Identifier == updateProfile.Identifier);

              if (p == null)
              {
                  Console.WriteLine($"Unable to find matching profile for {updateProfile.Identifier}");
                  return false;
              }

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
}