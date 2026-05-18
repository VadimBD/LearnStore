namespace LearnStore.Web.Api.Models.Author
{
    public record class AuthorResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
    }
}
