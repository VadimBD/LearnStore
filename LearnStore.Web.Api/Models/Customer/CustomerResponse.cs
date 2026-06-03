using LearnStore.Application.DTO;

namespace LearnStore.Web.Api.Models.Customer
{
    public record class CustomerResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<ProductDto> PurchasedProducts { get; set; } = [];
    }
}
