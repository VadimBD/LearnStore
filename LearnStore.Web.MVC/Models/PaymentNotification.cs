using LearnStore.Domain.Enums;

namespace LearnStore.Web.MVC.Models
{
    public class PaymentNotification
    {
        public string OrderId { get; set; } = string.Empty;
        public string TransactionId { get; set; }= string.Empty;
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Signature { get; set; } = string.Empty;
    }
}
