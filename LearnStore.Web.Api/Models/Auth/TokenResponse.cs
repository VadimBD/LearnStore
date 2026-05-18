namespace LearnStore.Web.Api.Models.Auth
{
    public record class TokenResponse
    {
        public bool IsSuccess { get; init; }
        public string Error { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
    }
}
