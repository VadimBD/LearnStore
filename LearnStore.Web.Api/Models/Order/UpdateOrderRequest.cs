using LearnStore.Application.DTO;

namespace LearnStore.Web.Api.Models.Order
{
    public record class UpdateOrderRequest
    {
        public Guid Id { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = [];
        public CustomerDto? Customer { get; set; }

        public ICollection<PaymentDto> Payments { get; set; } = [];
    }
}
