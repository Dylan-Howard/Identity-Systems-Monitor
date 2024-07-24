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
    [Column("service_id")]
    [JsonPropertyName("serviceid")]
    public Guid ServiceId { get; set; }
    [Column("start_time")]
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }
    [Column("end_time")]
    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }
    [Column("notes")]
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }
    [Column("active")]
    [JsonPropertyName("active")]
    public bool Active { get; set; }
}