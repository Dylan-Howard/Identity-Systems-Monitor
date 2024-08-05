
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
    [Column("service_type")]
    [JsonPropertyName("serviceType")]
    public string? ServiceType { get; set; }
    [Column("username")]
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    [Column("password")]
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    [Column("base_url")]
    [JsonPropertyName("baseUrl")]
    public string? BaseUrl { get; set; }
    [Column("token_url")]
    [JsonPropertyName("tokenUrl")]
    public string? TokenUrl { get; set; }
}

public class ServiceList
{
  [Key]
  [JsonPropertyName("data")]
  public required List<Service> Services { get; set; }
  [JsonPropertyName("total")]
  public int Total { get; set; }
}
