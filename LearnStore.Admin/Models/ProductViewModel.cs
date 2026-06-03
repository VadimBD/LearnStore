using LearnStore.Domain.Entities;

namespace LearnStore.Admin.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int AuthorId { get; set; }
        public string SellerId { get; set; }=string.Empty;  
        public ICollection<Product> ChildProducts { get; set; } = [];

        public int CategoryId { get; set; }

        public bool IsActive { get; set; }
        public decimal Price { get; set; }
    }
}
