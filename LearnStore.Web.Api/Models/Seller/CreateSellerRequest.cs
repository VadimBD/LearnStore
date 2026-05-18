namespace LearnStore.Web.Api.Models.Seller
{
    public record class CreateSellerRequest
    {
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
