using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prognosis.Models;

[Table("profile")]
public class Profile
{
    [Key]
    [Column("profile_id")]
    [JsonPropertyName("id")]
    public Guid ProfileId { get; set; }
    [Column("identifier")]
    [JsonPropertyName("identifier")]
    public required string Identifier { get; set; }
    [Column("first_name")]
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }
    [Column("middle_name")]
    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }
    [Column("last_name")]
    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }
    [Column("email")]
    [JsonPropertyName("email")]
    public required string Email { get; set; }
    [Column("birthdate")]
    [JsonPropertyName("birthdate")]
    public DateOnly Birthdate { get; set; }
    [Column("status")]
    [JsonPropertyName("status")]
    public bool Status { get; set; }
    [Column("claimed")]
    [JsonPropertyName("claimed")]
    public bool Claimed { get; set; }
    [Column("locked")]
    [JsonPropertyName("locked")]
    public bool Locked { get; set; }
    [Column("mfa_method")]
    [JsonPropertyName("mfaMethod")]
    public required string MfaMethod { get; set; }
}

public class ProfileShow : Profile
{
  public required List<ProfileLink> Links { get; set; }
}

public class ProfileList
{
  [Key]
  [JsonPropertyName("data")]
  public required List<Profile> Profiles { get; set; }
  [JsonPropertyName("total")]
  public int Total { get; set; }
}
