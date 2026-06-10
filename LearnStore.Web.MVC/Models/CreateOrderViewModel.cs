using LearnStore.Application.DTO;

namespace LearnStore.Web.MVC.Models
{
    public class CreateOrderViewModel
    {
        public IEnumerable<CartLine> Items { get; set; } = Enumerable.Empty<CartLine>();
        public decimal TotalAmount {  get; set; }
    }
}
