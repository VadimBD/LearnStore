using LearnStore.Application.DTO;
using LearnStore.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace LearnStore.Web.MVC.Models
{
    public class Cart
    {
        private List<CartLine> LineCollection = new List<CartLine>();

        public virtual void AddItem(ProductDto product)
        {
            CartLine line = LineCollection.Where(p => p.Product.Id == product.Id).FirstOrDefault();

            if (line == null)
            {
                LineCollection.Add(new CartLine { Product = product});
            }
            
        }

        public virtual void RemoveLine(int productId) => LineCollection.RemoveAll(l => l.Product.Id == productId);
        public virtual decimal ComputeTotalValue() => LineCollection.Sum(e => e.Product.Price);
        public virtual void Clear() => LineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => LineCollection;
    }
    public class CartLine
    {
        public int CartLineId { get; set; }
        [Required]
        public ProductDto Product { get; set; }
    }
}
