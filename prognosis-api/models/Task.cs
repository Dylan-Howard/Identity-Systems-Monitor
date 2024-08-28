using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prognosis.Models;

[Table("task")]
public class Task
{
    [Key]
    [Column("task_id")]
    [JsonPropertyName("id")]
    public Guid TaskId { get; set; }
    [Column("job_id")]
    [JsonPropertyName("jobId")]
    public Guid JobId { get; set; }
    [Column("start_time")]
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }
    [Column("end_time")]
    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }
    [Column("notes")]
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }
    [Column("active")]
    [JsonPropertyName("active")]
    public bool Active { get; set; }
}

public class TaskListItem : Task
{
    [JsonPropertyName("serviceId")]
    public required Guid ServiceId { get; set; }
    [JsonPropertyName("serviceName")]
    public required string ServiceName { get; set; }
}

public class TaskList
{
    [Key]
    [JsonPropertyName("data")]
    public required List<TaskListItem> Tasks { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}