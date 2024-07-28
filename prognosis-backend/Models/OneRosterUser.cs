using System.Text.Json.Serialization;

namespace prognosis_backend.models
{
    public class OneRosterUser
    {
        [JsonPropertyName("sourcedId")]
        public Guid SourcedId { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("dateLastModified")]
        public DateTime DateLastModified { get; set; }
        [JsonPropertyName("metadata")]
        public OneRosterUserMetadata? Metadata { get; set; }
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        [JsonPropertyName("enabledUser")]
        public string? EnabledUser { get; set; }
        [JsonPropertyName("sms")]
        public string? SMS { get; set; }
        [JsonPropertyName("givenName")]
        public string? GivenName { get; set; }
        [JsonPropertyName("familyName")]
        public string? FamilyName { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("userIds")]
        public IList<OneRosterUserId>? UserIds { get; set; }
        [JsonPropertyName("roles")]
        public IList<OneRosterUserRole>? Roles { get; set; }
        [JsonPropertyName("agents")]
        public IList<OneRosterUser>? Agents { get; set; }

        public static implicit operator string?(OneRosterUser? v)
        {
          if (v == null) {
            return null;
          }

          string objString = $"{{\n" +
            $"\tSourcedId: {v.SourcedId},\n" +
            $"\tStatus: {v.Status},\n" +
            $"\tDateLastModified: {v.DateLastModified},\n" +
            $"\tUsername: {v.Username},\n" +
            $"\tEnabledUser: {v.EnabledUser},\n" +
            $"\tSMS: {v.SMS},\n" +
            $"\tGivenName: {v.GivenName},\n" +
            $"\tFamilyName: {v.FamilyName},\n" +
            $"\tEmail: {v.Email},\n" +
            $"\tMetadata: {v.Metadata},\n" +
          "}}";

          return objString;
        }
    }
    public class OneRosterUserRelationship {
        [JsonPropertyName("sourcedId")]
        public Guid SourcedId { get; set; }
        [JsonPropertyName("guardian")]
        public bool Guardian { get; set; }
        [JsonPropertyName("relationshipType")]
        public required string Type { get; set; }

        public static implicit operator string?(OneRosterUserRelationship? v)
        {
          if (v == null) {
            return null;
          }

          string objString = $"{v.SourcedId}: {{\n" +
            $"\tSourcedId: {v.SourcedId},\n" +
            $"\tGuardian: {v.Guardian},\n" +
            $"\tType: {v.Type},\n" +
          "}}";

          return objString;
        }
    }
    public class OneRosterUserMetadata {
        [JsonPropertyName("ic_relationships")]
        public IList<OneRosterUserRelationship>? Relationships { get; set; }
        [JsonPropertyName("ic_indexSuffix")]
        public required string IndexSuffix { get; set; }
        [JsonPropertyName("ic.legacySourcedId")]
        public required string LegacySourcedId { get; set; }

        public static implicit operator string?(OneRosterUserMetadata? m)
        {
          if (m == null) {
            return null;
          }

          string objString = $"{{\n" +
            $"\tOneRosterUserRelationships: {m.Relationships},\n" +
          "}}";

          return objString;
        }
    }
    public class OneRosterUserAgent {
        [JsonPropertyName("sourcedId")]
        public required string SourcedId { get; set; }
        [JsonPropertyName("href")]
        public required string Href { get; set; }
        [JsonPropertyName("type")]
        public required string Type { get; set; }
    }
    public class OneRosterUserRoleOrg
    {
        [JsonPropertyName("href")]
        public required string Href { get; set; }
        [JsonPropertyName("sourcedId")]
        public Guid SourcedId { get; set; }
        [JsonPropertyName("type")]
        public required string Type { get; set; }
    }
    public class OneRosterUserRole {
        [JsonPropertyName("beginDate")]
        public DateOnly BeginDate { get; set; }
        [JsonPropertyName("roleType")]
        public required string RoleType { get; set; }
        [JsonPropertyName("role")]
        public required string Role { get; set; }
        [JsonPropertyName("org")]
        public required OneRosterUserRoleOrg Org { get; set; }
    }
    public class OneRosterUserId {
        [JsonPropertyName("identifier")]
        public required string Identifier { get; set; }
        [JsonPropertyName("type")]
        public required string Type { get; set; }
    }
    public class OneRosterTokenResponse
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
        [JsonPropertyName("scope")]
        public required string Scope { get; set; }
        [JsonPropertyName("token_type")]
        public required string TokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
    public class OneRosterUsersResponse {
        [JsonPropertyName("users")]
        public required List<OneRosterUser> Users { get; set; }
    }
    public class OneRosterConnectionSettings
    {
        public required string BaseUrl { get; set; }
        public required string TokenUrl { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}

