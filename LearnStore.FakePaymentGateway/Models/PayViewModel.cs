namespace LearnStore.FakePaymentGateway.Models
{
    public class PayViewModel
    {
        public string OrderId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string ReturnUrl { get; set; } = string.Empty;
        public string NotifyUrl { get; set; } = string.Empty;
    }
}
