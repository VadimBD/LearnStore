namespace LearnStore.Web.Api.Models.Auth
{
    public record class RoleOperationResponse
    {
        public bool Success { get; init; }
        public string ErrorMessage { get; init; } = string.Empty;
        public string RoleName { get; init; } = string.Empty;
        public string UserId { get; init; } = string.Empty;
        public IEnumerable<string> CurrentRoles { get; init; } = [];
    }
}
