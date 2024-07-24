
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prognosis.Models;

[Table("service")]
public class Service
{
    [Key]
    [Column("service_id")]
    [JsonPropertyName("id")]
    public Guid ServiceId { get; set; }
    [Column("name")]
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}

public class ServiceList
{
  [Key]
  [JsonPropertyName("data")]
  public required List<Service> Services { get; set; }
  [JsonPropertyName("total")]
  public int Total { get; set; }
}
