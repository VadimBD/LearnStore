using LearnStore.Application.DTO;

namespace LearnStore.Web.Api.Models.Order
{
    public record class CreateOrderRequest
    {
        public CustomerDto? Customer { get; init; }
        public ICollection<OrderItemDto> Items { get; init; } = [];
    }
}
