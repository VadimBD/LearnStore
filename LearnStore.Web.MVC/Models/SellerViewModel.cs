using LearnStore.Application.DTO;

namespace LearnStore.Web.MVC.Models
{
    public class SellerViewModel
    {
        public SellerDto Seller { get; set; }
        public IEnumerable<ProductDto> Products { get; set; } = Enumerable.Empty<ProductDto>();

    }
}
