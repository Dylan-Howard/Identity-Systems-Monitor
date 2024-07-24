using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prognosis_backend.models;

[Table("link")]
public class Link
{
    [Key]
    [Column("link_id")]
    [JsonPropertyName("id")]
    public Guid LinkId { get; set; }
    [Column("profile_id")]
    [JsonPropertyName("profileId")]
    public Guid ProfileId { get; set; }
    [Column("service_id")]
    [JsonPropertyName("serviceId")]
    public Guid ServiceId { get; set; }
    [Column("service_identifier")]
    [JsonPropertyName("identifier")]
    public required string ServiceIdentifier { get; set; }
    [Column("active")]
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    [Column("first_name")]
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }
    [Column("last_name")]
    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }
    [Column("address")]
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [Column("phone")]
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
    [Column("photo_url")]
    [JsonPropertyName("photoUrl")]
    public string? PhotoUrl { get; set; }
    [Column("org_unit_path")]
    [JsonPropertyName("orgUnitPath")]
    public required string OrgUnitPath { get; set; }
    [Column("organization")]
    [JsonPropertyName("organization")]
    public string? Organization { get; set; }
    [Column("created_date")]
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [Column("last_activity")]
    [JsonPropertyName("lastActivity")]
    public DateTime LastActivity { get; set; }
    public static implicit operator string?(Link? l)
    {
      if (l == null) {
        return null;
      }

      string objString = $"{l.LinkId}: {{\n" +
        $"\tProfileId: \"{l.ProfileId}\",\n" +
        $"\tServiceId: \"{l.ServiceId}\",\n" +
        $"\tServiceIdentifier: \"{l.ServiceIdentifier}\",\n" +
        $"\tActive: \"{l.Active}\",\n" +
        $"\tFirstName: \"{l.FirstName}\",\n" +
        $"\tLastName: \"{l.LastName}\",\n" +
        $"\tAddress: \"{l.Address}\",\n" +
        $"\tPhone: \"{l.Phone}\",\n" +
        $"\tPhotoUrl: \"{l.PhotoUrl}\",\n" +
        $"\tOrgUnitPath: \"{l.OrgUnitPath}\",\n" +
        $"\tOrganization: \"{l.Organization}\",\n" +
        $"\tCreatedDate: \"{l.CreatedDate}\",\n" +
        $"\tLastActivity: \"{l.LastActivity}\",\n" +
      "}}";

      return objString;
    }
}
