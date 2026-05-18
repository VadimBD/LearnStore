namespace LearnStore.Web.Api.Models.Auth
{
    public record class RoleOperationRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
