using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prognosis.Models;

[Table("agent")]
public class Agent
{
    [Key]
    [Column("agent_id")]
    [JsonPropertyName("id")]
    public Guid AgentId { get; set; }
    [Column("username")]
    public required string Username { get; set; }
    [Column("password")]
    public required string Password { get; set; }
}

public class AgentList
{
    [Key]
    [JsonPropertyName("data")]
    public required List<Agent> Agents { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}