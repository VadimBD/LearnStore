using LearnStore.Application.DTO;
using LearnStore.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearnStore.Admin.Models
{
    public class OrderViewModel
    {
        public OrderDto Order { get; set; } = new OrderDto();
        public IEnumerable<ProductDto> Products { get; set; } = [];
        public IEnumerable<SelectListItem> ProductsSLI { get => Products.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }); }
        public decimal TotalPrice => Order.Items.Sum(item => item.PriceAtOrderTime * item.Quantity);
        public IEnumerable<SelectListItem> OrderStateSLI { get => Enum.GetValues<OrderState>().Cast<OrderState>().Select(l => new SelectListItem { Text = l.ToString(), Value = ((int)l).ToString() }).ToList(); }
    }
}
