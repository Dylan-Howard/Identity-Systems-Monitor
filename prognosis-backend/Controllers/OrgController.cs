
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prognosis_backend.models;

namespace prognosis_backend;

public class OrgController
{
    public static async Task<List<Org>> FetchOrgRecords(PrognosisConnectionSettings settings, int retryCount)
    {
        List<Org> orgs = [];
        if (retryCount == 0) {
            return orgs;
        }

        try
        {
            var db = new PrognosisContext(settings);
            orgs = await db.Orgs.ToListAsync();
        }
        catch (SqlException e)
        {
            if (e.Number == -2) {
                Console.WriteLine("Connection timed out. Retrying...");
                return await FetchOrgRecords(settings, retryCount - 1);
            } else {
                Console.WriteLine(e.ToString());
            }
        }
            
        return orgs;
    }

    public static async Task<bool> AddOrgRecord(PrognosisConnectionSettings settings, Org addOrg, int retryCount)
    {
        if (retryCount == 0) {
            return false;
        }

        try
        {
            var db = new PrognosisContext(settings);
            await db.AddAsync(addOrg);
            await db.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            if (e.Number == -2) {
                Console.WriteLine("Connection timed out. Retrying...");
                return await AddOrgRecord(settings, addOrg, retryCount - 1);
            }

            Console.WriteLine(e.ToString());
            return false;
        }

        return true;
    }
    static RecordChanges HasOrgRecordChanged(Org org1, Org org2)
    {
        List<string> changedFields = [];

        if (org1.Status != org2.Status) {
            org1.Status = org2.Status;
            changedFields.Add("Status");
        }
          if (org1.DateLastModified != org2.DateLastModified) {
            org1.DateLastModified = org2.DateLastModified;
            changedFields.Add("DateLastModified");
        }
          if (org1.Name != org2.Name) {
            org1.Name = org2.Name;
            changedFields.Add("Name");
        }
          if (org1.Type != org2.Type) {
            org1.Type = org2.Type;
            changedFields.Add("Type");
        }
        if (org1.Address != org2.Address) {
            org1.Address = org2.Address;
            changedFields.Add("Address");
        }
        if (org1.City != org2.City) {
            org1.City = org2.City;
            changedFields.Add("City");
        }
        if (org1.State != org2.State) {
            org1.State = org2.State;
            changedFields.Add("State");
        }
        if (org1.Zip != org2.Zip) {
            org1.Zip = org2.Zip;
            changedFields.Add("Zip");
        }
            
        return new RecordChanges {
            ChangedFields = changedFields
        };
    }
    public static async Task<bool> UpdateOrgRecord(PrognosisConnectionSettings settings, Org updateOrg, int retryCount)
    {
        if (retryCount == 0) {
            return false;
        }

        try
        {
            var db = new PrognosisContext(settings);

            Org? org = await db.Orgs.FirstOrDefaultAsync(
                (o) => Equals(o.Identifier, updateOrg.Identifier));

            if (org == null)
            {
                return false;
            }

            RecordChanges changes = HasOrgRecordChanged(updateOrg, org);

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
                return await UpdateOrgRecord(settings, updateOrg, retryCount - 1);
            }
            Console.WriteLine(e.ToString());
            Console.WriteLine(updateOrg);
            return false;
        }

        return true;
    }
}