
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Prognosis.Models;

public class Dashboard
{
  [Key]
  public required string Id { get; set; }
  [JsonPropertyName("totals")]
  public required IList<DashboardTotal> Totals { get; set; }
  [JsonPropertyName("trends")]
  public required IList<DashboardTrend> Trends { get; set; }
  [JsonPropertyName("changes")]
  public required IList<DashboardChange> Changes { get; set; }
}

public class DashboardTotal
{
    [Key]
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("title")]
    public required string Title { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class DashboardTrendDataPoint
{
    [Key]
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; }
}
public class DashboardTrend
{
    [Key]
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("title")]
    public required string Title { get; set; }
    [JsonPropertyName("data")]
    public required IList<DashboardTrendDataPoint> Data { get; set; }
}

public class DashboardChange
{
    [Key]
    [JsonPropertyName("user")]
    public required string User { get; set; }
    [JsonPropertyName("currentId")]
    public required string CurrentId { get; set; }
    [JsonPropertyName("formerId")]
    public required string FormerId { get; set; }
    [JsonPropertyName("timestamp")]
    public required DateTime Timestamp { get; set; }
}