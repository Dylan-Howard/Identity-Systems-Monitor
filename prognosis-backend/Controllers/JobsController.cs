using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prognosis_backend.models;

namespace prognosis_backend;

public static class JobsController
{
    public static async Task<List<Job>?> GetJobs(PrognosisConnectionSettings settings)
    {
        List<Job> jobs = [];
        try
        {
            var db = new PrognosisContext(settings);

            jobs = await db.Jobs.ToListAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return null;
        }

        return jobs;
    }
    public static async Task<IList<Job>?> GetReadyJobs(PrognosisConnectionSettings settings)
    {
        List<Job>? jobs = await GetJobs(settings);

        if (jobs == null)
        {
            return [];
        }

        jobs = jobs.FindAll((j) => DateTime.Compare(j.NextRuntime, DateTime.Now) < 0);
        
        return jobs;
    }
    public static async Task<bool> PutJob(PrognosisConnectionSettings settings, Job job)
    {
        try
        {
            var db = new PrognosisContext(settings);
            db.Entry(job).State = EntityState.Modified;
            
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return true;
    }
}