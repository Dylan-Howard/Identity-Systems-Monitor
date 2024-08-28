
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prognosis_backend.models;

namespace prognosis_backend;

public class ClassController
{
    public static async Task<List<Class>> FetchClassRecords(PrognosisConnectionSettings settings)
    {
        List<Class> classes = [];
  
        try
        {
            var db = new PrognosisContext(settings);
            classes = await db.Classes.ToListAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
            
        return classes;
    }

    public static async Task<bool> AddClassRecord(PrognosisConnectionSettings settings, Class addClass)
    {
        try
        {
            var db = new PrognosisContext(settings);
            await db.AddAsync(addClass);
            await db.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }

        return true;
    }
    static RecordChanges HasClassRecordChanged(Class class1, Class class2)
    {
        List<string> changedFields = [];

        if (class1.Status != class2.Status) {
            class1.Status = class2.Status;
            changedFields.Add("Status");
        }
          if (class1.DateLastModified != class2.DateLastModified) {
            class1.DateLastModified = class2.DateLastModified;
            changedFields.Add("DateLastModified");
        }
          if (class1.Title != class2.Title) {
            class1.Title = class2.Title;
            changedFields.Add("Title");
        }
          if (class1.ClassType != class2.ClassType) {
            class1.ClassType = class2.ClassType;
            changedFields.Add("ClassType");
        }
        if (class1.ClassCode != class2.ClassCode) {
            class1.ClassCode = class2.ClassCode;
            changedFields.Add("ClassCode");
        }
        if (class1.Location != class2.Location) {
            class1.Location = class2.Location;
            changedFields.Add("Location");
        }
        if (class1.OrgSourcedId != class2.OrgSourcedId) {
            class1.OrgSourcedId = class2.OrgSourcedId;
            changedFields.Add("OrgSourcedId");
        }
            
        return new RecordChanges {
            ChangedFields = changedFields
        };
    }
    public static async Task<bool> UpdateClassRecord(PrognosisConnectionSettings settings, Class updateClass)
    {
        try
        {
            var db = new PrognosisContext(settings);

            Class? match = await db.Classes.FirstOrDefaultAsync(
                (c) => Equals(c.Identifier, updateClass.Identifier));

            if (match == null)
            {
                return false;
            }

            RecordChanges changes = HasClassRecordChanged(updateClass, match);

            if (changes.ChangedFields.Count == 0)
            {
                return true;
            }
                
            await db.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }

        return true;
    }
}