using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prognosis_backend.models;

[Table("task")]
public class Task
{
    [Key]
    [Column("task_id")]
    public Guid TaskId { get; set; }
    [Column("job_id")]
    public Guid JobId { get; set; }
    [Column("start_time")]
    public required DateTime StartTime { get; set; }
    [Column("end_time")]
    public DateTime? EndTime { get; set; }
    [Column("notes")]
    public string? Notes { get; set; }
    [Column("active")]
    public required bool Active { get; set; }

    public static implicit operator string?(Task? v)
    {
        if (v == null) {
            return null;
        }

        string objString = $"{{\n" +
            $"\tTaskId: {v.TaskId},\n" +
            $"\tJobId: {v.JobId},\n" +
            $"\tStartTime: {v.StartTime},\n" +
            $"\tEndTime: {v.EndTime},\n" +
            $"\tNotes: {v.Notes},\n" +
            $"\tActive: {v.Active},\n" +
            "}}";

        return objString;
    }
}
