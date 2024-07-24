using System.Text.Json.Serialization;

namespace prognosis_backend.models
{
    public class RapidIdentityUser
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        [JsonPropertyName("createTimestamp")]
        public string? CreateTimestamp { get; set; }
        [JsonPropertyName("employeeType")]
        public required string EmployeeType { get; set; }
        [JsonPropertyName("givenName")]
        public required string FirstName { get; set; }
        [JsonPropertyName("idautoPersonMiddleName")]
        public string? MiddleName { get; set; }
        [JsonPropertyName("sn")]
        public required string LastName { get; set; }
        [JsonPropertyName("idautoDisabled")]
        public string? IsDisabled { get; set; }
        [JsonPropertyName("idautoLocked")]
        public bool IsLocked { get; set; }
        [JsonPropertyName("idautoPersonClaimFlag")]
        public bool ClaimFlag { get; set; }
        [JsonPropertyName("idautoPersonMfaMethod")]
        public string? MfaMethod { get; set; }
        [JsonPropertyName("idautoPersonStuID")]
        public int? StudentId { get; set; }
        [JsonPropertyName("idautoPersonHRID")]
        public int? EmployeeId { get; set; }
        [JsonPropertyName("idautoPersonHomeEmail")]
        public string? HomeEmail { get; set; }
        [JsonPropertyName("idautoPersonGender")]
        public required string Gender { get; set; }
        [JsonPropertyName("idautoPersonExtBool2")]
        public required string IsWelcomed { get; set; }
        [JsonPropertyName("idautoPersonRenameUsername")]
        public string? RenameUsername { get; set; }
        [JsonPropertyName("idautoPersonRenameFlagDate")]
        public string? RenameDate { get; set; }
        [JsonPropertyName("idautoPersonSystem4ID")]
        public string? InfiniteCampusGUID { get; set; }
        [JsonPropertyName("mail")]
        public required string Email { get; set; }
        [JsonPropertyName("modifyTimestamp")]
        public required string DateLastModified { get; set; }
        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }
        [JsonPropertyName("idautoPersonSourceStatus")]
        public string? IsActive { get; set; }
        [JsonPropertyName("idautoPersonBirthdate")]
        public required string BirthDate { get; set; }
        [JsonPropertyName("pwdChangedTime")]
        public required string PasswordChangedDate { get; set; }

      public static implicit operator string?(RapidIdentityUser? v)
      {
        if (v == null) {
          return null;
        }

        string objString = $"{v.Id}: {{\n" +
          $"\tfirstName: {v.FirstName},\n" +
          $"\tlastName: {v.LastName},\n" +
          $"\tIsDisabled: {v.IsDisabled},\n" +
          $"\tIsLocked: {v.IsLocked},\n" +
          $"\tClaimFlag: {v.ClaimFlag},\n" +
          $"\tMfaMethod: {v.MfaMethod},\n" +
          $"\tStudentId: {v.StudentId},\n" +
          $"\tEmployeeId: {v.EmployeeId},\n" +
          $"\tHomeEmail: {v.HomeEmail},\n" +
          $"\tGender: {v.Gender},\n" +
          $"\tIsWelcomed: {v.IsWelcomed},\n" +
          $"\tRenameUsername: {v.RenameUsername},\n" +
          $"\tRenameDate: {v.RenameDate},\n" +
          $"\tInfiniteCampusGUID: {v.InfiniteCampusGUID},\n" +
          $"\tEmail: {v.Email},\n" +
          $"\tDateLastModified: {v.DateLastModified},\n" +
          $"\tmobile: {v.Mobile},\n" +
          $"\tIsActive: {v.IsActive},\n" +
          $"\tBirthDate: {v.BirthDate},\n" +
        "}}";

        return objString;
      }

      public static implicit operator Profile?(RapidIdentityUser u)
      {
          int? identifier = u.EmployeeType == "Staff" ? u.EmployeeId : u.StudentId;

          return new Profile {
              Identifier = identifier.ToString() ?? "undefined",
              FirstName = u.FirstName,
              MiddleName = u.MiddleName,
              LastName = u.LastName ?? "",
              Email = u.Email,
              Birthdate = u.BirthDate != null ? DateOnly.Parse(u.BirthDate) : new DateOnly(1700, 1, 1),
              Gender = u.Gender ?? "?",
              Status = u.IsActive == "active",
              Claimed = u.ClaimFlag,
              Locked = u.IsLocked,
              MfaMethod = u.MfaMethod ?? "None",
          };
      }
    }

    public class RapidIdentityUserResponse {
      [JsonPropertyName("data")]
      public required RapidIdentityUser Data { get; set; }
    }
    public class RapidIdentityUsersResponse {
      [JsonPropertyName("data")]
      public required List<RapidIdentityUser> Data { get; set; }
    }
}

