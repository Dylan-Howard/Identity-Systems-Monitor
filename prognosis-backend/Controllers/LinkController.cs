
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prognosis_backend.models;

namespace prognosis_backend;

public class LinkController
{
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
}