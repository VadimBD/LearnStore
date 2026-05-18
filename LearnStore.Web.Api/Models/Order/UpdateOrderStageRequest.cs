using LearnStore.Domain.Enums;

namespace LearnStore.Web.Api.Models.Order
{
    public record class UpdateOrderStageRequest
    {
        public Guid OrderId { get; set; } = Guid.Empty;
        public OrderState OrderState { get; set; }
    }
}
