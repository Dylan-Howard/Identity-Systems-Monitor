using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prognosis_backend.models;

[Table("service")]
public class Service
{
    [Key]
    [Column("service_id")]
    public Guid ServiceId { get; set; }
    [Column("name")]
    public required string Name { get; set; }
}
