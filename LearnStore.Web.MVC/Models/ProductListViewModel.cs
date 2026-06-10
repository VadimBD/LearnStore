using LearnStore.Application.DTO;

namespace LearnStore.Web.MVC.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
