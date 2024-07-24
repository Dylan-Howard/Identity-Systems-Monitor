using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace prognosis_backend.models
{
    public class UserRecord
    {
        public required string Id { get; set; }
        public string? CreateTimestamp { get; set; }
        public required string EmployeeType { get; set; }
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }
        public string? IsDisabled { get; set; }
        public bool IsLocked { get; set; }
        public int ClaimCode { get; set; }
        public required string ClaimFlag { get; set; }
        public string? MfaMethod { get; set; }
        public int? StudentId { get; set; }
        public int? EmployeeId { get; set; }
        public string? HomeEmail { get; set; }
        public required string Gender { get; set; }
        public required string IsWelcomed { get; set; }
        public required string JobCode { get; set; }
        public required string JobTitle { get; set; }
        public required string SchoolCode { get; set; }
        public required string SchoolName { get; set; }
        public required string LocationCode { get; set; }
        public required string LocationName { get; set; }
        public string? RenameUsername { get; set; }
        public string? RenameDate { get; set; }
        public required string ImmuableId { get; set; }
        public string? InfiniteCampusGUID { get; set; }
        public required string Email { get; set; }
        public required string DateLastModified { get; set; }
        public string? Mobile { get; set; }
        public string? IsActive { get; set; }
        public required string BirthDate { get; set; }

        public static implicit operator string?(UserRecord? u)
        {
          if (u == null) {
            return null;
          }

          string objString = $"{u.Id}: {{\n" +
            $"\tfirstName: {u.FirstName},\n" +
            $"\tlastName: {u.LastName},\n" +
            $"\tIsDisabled: {u.IsDisabled},\n" +
            $"\tIsLocked: {u.IsLocked},\n" +
            $"\tClaimCode: {u.ClaimCode},\n" +
            $"\tClaimFlag: {u.ClaimFlag},\n" +
            $"\tMfaMethod: {u.MfaMethod},\n" +
            $"\tStudentId: {u.StudentId},\n" +
            $"\tEmployeeId: {u.EmployeeId},\n" +
            $"\tHomeEmail: {u.HomeEmail},\n" +
            $"\tGender: {u.Gender},\n" +
            $"\tIsWelcomed: {u.IsWelcomed},\n" +
            $"\tJobCode: {u.JobCode},\n" +
            $"\tJobTitle: {u.JobTitle},\n" +
            $"\tSchoolCode: {u.SchoolCode},\n" +
            $"\tSchoolName: {u.SchoolName},\n" +
            $"\tLocationCode: {u.LocationCode},\n" +
            $"\tLocationName: {u.LocationName},\n" +
            $"\tRenameUsername: {u.RenameUsername},\n" +
            $"\tRenameDate: {u.RenameDate},\n" +
            $"\tImmuableId: {u.ImmuableId},\n" +
            $"\tInfiniteCampusGUID: {u.InfiniteCampusGUID},\n" +
            $"\tEmail: {u.Email},\n" +
            $"\tDateLastModified: {u.DateLastModified},\n" +
            $"\tmobile: {u.Mobile},\n" +
            $"\tIsActive: {u.IsActive},\n" +
            $"\tBirthDate: {u.BirthDate},\n" +
          "}}";

          return objString;
        }
      }
}
