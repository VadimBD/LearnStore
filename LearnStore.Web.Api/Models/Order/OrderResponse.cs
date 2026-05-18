using LearnStore.Domain.Enums;

namespace LearnStore.Web.Api.Models.Order
{
    public record class OrderResponse
    {
        public Guid Id { get; init; }
        public CustomerResponse? Customer { get; init; }
        public string Comments { get; set; } = string.Empty;
        public int PersonCount { get; set; } = 1;
        public DateTime OrderDate { get; set; }
     
        public List<OrderItemResponse> Items { get; init; } = [];
        public OrderState OrderStage { get; set; } 
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
