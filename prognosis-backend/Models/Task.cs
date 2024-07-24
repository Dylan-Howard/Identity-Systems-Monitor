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
    [Column("service_id")]
    public Guid ServiceId { get; set; }
    [Column("start_time")]
    public DateTime StartTime { get; set; }
    [Column("end_time")]
    public DateTime EndTime { get; set; }
    [Column("notes")]
    public required string Notes { get; set; }
    [Column("active")]
    public bool Active { get; set; }
}
