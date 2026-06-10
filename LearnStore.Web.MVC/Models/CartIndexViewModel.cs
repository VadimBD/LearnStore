namespace LearnStore.Web.MVC.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
