using LearnStore.Application.DTO;

namespace LearnStore.Web.MVC.Models
{
    public class OrderViewModel
    {
        public IEnumerable<OrderItemDto> Items { get; set; } = Enumerable.Empty<OrderItemDto>();
    }
}
