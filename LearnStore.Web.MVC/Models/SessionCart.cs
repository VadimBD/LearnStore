using LearnStore.Application.DTO;
using LearnStore.Domain.Entities;
using LearnStore.Web.MVC.Extensions;
using Newtonsoft.Json;

namespace LearnStore.Web.MVC.Models
{
    public class SessionCart:Cart
    {
        public static Cart GetCart(IServiceProvider service)
        {
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession Session { get; set; }
        public override void AddItem(ProductDto product)
        {
            base.AddItem(product);
            Session.SetJson("Cart", this);
        }

        public override void RemoveLine(int productId)
        {
            base.RemoveLine(productId);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
