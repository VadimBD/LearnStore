using LearnStore.Domain.Entities;
using LearnStore.Domain.Enums;

namespace LearnStore.Admin.Models
{
    public class OrdersFilter
    {
        public string CustomerId { get; init; } = string.Empty;
    }
}
