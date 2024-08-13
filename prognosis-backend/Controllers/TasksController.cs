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
}