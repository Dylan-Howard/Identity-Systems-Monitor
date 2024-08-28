
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prognosis_backend.models;

namespace prognosis_backend;

public class EnrollmentController
{
    public static async Task<List<Enrollment>> FetchEnrollmentRecords(PrognosisConnectionSettings settings)
    {
        List<Enrollment> enrollments = [];
        try
        {
            var db = new PrognosisContext(settings);
            enrollments = await db.Enrollments.ToListAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
            
        return enrollments;
    }

    public static async Task<bool> AddEnrollmentRecord(PrognosisConnectionSettings settings, Enrollment addEnrollment)
    {
        try
        {
            var db = new PrognosisContext(settings);
            
            await db.AddAsync(addEnrollment);
            await db.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }

        return true;
    }
    static RecordChanges HasEnrollmentRecordChanged(Enrollment enrollment1, Enrollment enrollment2)
    {
        List<string> changedFields = [];

        if (enrollment1.Status != enrollment2.Status) {
            enrollment1.Status = enrollment2.Status;
            changedFields.Add("Status");
        }
          if (enrollment1.DateLastModified != enrollment2.DateLastModified) {
            enrollment1.DateLastModified = enrollment2.DateLastModified;
            changedFields.Add("DateLastModified");
        }
          if (enrollment1.Role != enrollment2.Role) {
            enrollment1.Role = enrollment2.Role;
            changedFields.Add("Role");
        }
          if (enrollment1.Primary != enrollment2.Primary) {
            enrollment1.Primary = enrollment2.Primary;
            changedFields.Add("Primary");
        }
        if (enrollment1.BeginDate != enrollment2.BeginDate) {
            enrollment1.BeginDate = enrollment2.BeginDate;
            changedFields.Add("BeginDate");
        }
        if (enrollment1.EndDate != enrollment2.EndDate) {
            enrollment1.EndDate = enrollment2.EndDate;
            changedFields.Add("EndDate");
        }
        if (enrollment1.UserSourcedId != enrollment2.UserSourcedId) {
            enrollment1.UserSourcedId = enrollment2.UserSourcedId;
            changedFields.Add("UserSourcedId");
        }
        if (enrollment1.ClassSourcedId != enrollment2.ClassSourcedId) {
            enrollment1.ClassSourcedId = enrollment2.ClassSourcedId;
            changedFields.Add("ClassSourcedId");
        }
            
        return new RecordChanges {
            ChangedFields = changedFields
        };
    }
    public static async Task<bool> UpdateEnrollmentRecord(PrognosisConnectionSettings settings, Enrollment updateEnrollment)
    {
        try
        {
            var db = new PrognosisContext(settings);

            Enrollment? match = await db.Enrollments.FirstOrDefaultAsync(
                (c) => Equals(c.Identifier, updateEnrollment.Identifier));

            if (match == null)
            {
                return false;
            }

            RecordChanges changes = HasEnrollmentRecordChanged(updateEnrollment, match);

            if (changes.ChangedFields.Count == 0)
            {
                return true;
            }
                
            await db.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            Console.WriteLine(updateEnrollment);
            return false;
        }

        return true;
    }
}