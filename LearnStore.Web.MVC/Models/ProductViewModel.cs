using LearnStore.Application.DTO;
using LearnStore.Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace LearnStore.Web.MVC.Models
{
    public class ProductViewModel
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "NameRequired")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "AuthorIdRequired")]
        public int AuthorId { get; set; }
        
        public string? SellerId { get; set; }
        public ICollection<int> ChildProducts { get; set; } = [];

        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "CategoryIdRequired")]
        public int? CategoryId { get; set; }
        
        public bool IsActive { get; set; }=true;

        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "PriceRequired")]
        public decimal Price { get; set; }
    }
}
