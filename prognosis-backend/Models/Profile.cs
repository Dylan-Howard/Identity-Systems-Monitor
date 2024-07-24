using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prognosis_backend.models;

[Table("profile")]
public class Profile
{
    [Key]
    [Column("profile_id")]
    public Guid ProfileId { get; set; }
    [Column("identifier")]
    public required string Identifier { get; set; }
    [Column("first_name")]
    public required string FirstName { get; set; }
    [Column("middle_name")]
    public string? MiddleName { get; set; }
    [Column("last_name")]
    public required string LastName { get; set; }
    [Column("email")]
    public required string Email { get; set; }
    [Column("birthdate")]
    public DateOnly Birthdate { get; set; }
    [Column("gender")]
    public required string Gender { get; set; }
    [Column("status")]
    public bool Status { get; set; }
    [Column("claimed")]
    public bool Claimed { get; set; }
    [Column("locked")]
    public bool Locked { get; set; }
    [Column("mfa_method")]
    public string? MfaMethod { get; set; }

    public static implicit operator string?(Profile? p)
    {
      if (p == null) {
        return null;
      }

      string objString = $"{p.ProfileId}: {{\n" +
        $"\tIdentifier: {p.Identifier},\n" +
        $"\tfirstName: {p.FirstName},\n" +
        $"\tlastName: {p.LastName},\n" +
        $"\tMiddleName: {p.MiddleName},\n" +
        $"\tEmail: {p.Email},\n" +
        $"\tBirthdate: {p.Birthdate},\n" +
        $"\tGender: {p.Gender},\n" +
        $"\tStatus: {p.Status},\n" +
        $"\tClaimed: {p.Claimed},\n" +
        $"\tLocked: {p.Locked},\n" +
        $"\tMfaMethod: {p.MfaMethod},\n" +
      "}}";

      return objString;
    }
}
