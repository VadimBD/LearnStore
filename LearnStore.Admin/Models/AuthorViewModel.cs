namespace LearnStore.Admin.Models
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; } 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
    }
}
