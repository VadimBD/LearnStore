namespace LearnStore.Web.Api.Models.Auth
{
    public record class RegisterResponse
    {
        public bool IsSuccess { get; init; }
        public string UserId { get; init; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Error { get; init; } = string.Empty;
    }
}
