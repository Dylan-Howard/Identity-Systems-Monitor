using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace prognosis_backend;

public static class TasksController
{
    public static async Task<IList<models.Task>?> GetTasks(PrognosisConnectionSettings settings)
    {
        List<models.Task> tasks = [];
        try
        {
            var db = new PrognosisContext(settings);

            tasks = await db.Tasks.ToListAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return null;
        }

        return tasks;
    }
    public static async Task<bool> PutTask(PrognosisConnectionSettings settings, models.Task task)
    {
        try
        {
            var db = new PrognosisContext(settings);
            db.Entry(task).State = EntityState.Modified;
            
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return true;
    }
    public static async Task<models.Task?> PostTask(PrognosisConnectionSettings settings, models.Task task)
    {
        task.TaskId = Guid.NewGuid();

        Console.WriteLine(task);

        try
        {
            var db = new PrognosisContext(settings);
            db.Tasks.Add(task);
            await db.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }

        return task;
    }
}