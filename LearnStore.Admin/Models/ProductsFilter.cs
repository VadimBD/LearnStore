namespace LearnStore.Admin.Models
{
    public class ProductsFilter
    {
        public string SellerId { get; init; }= string.Empty;
        public int CategoryId { get; init; }
        public int AuthorId { get; init; }
        public bool IsActive { get; init; }
        public decimal Price { get; init; }

        public string ProductName { get; init; }=string.Empty;
    }
}
