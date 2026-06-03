using LearnStore.Application.DTO;
using System.ComponentModel.DataAnnotations;

namespace LearnStore.Web.MVC.Models
{
    public class ProductViewModel
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public int AuthorId { get; set; }
        
        public string? SellerId { get; set; }
        public ICollection<int> ChildProducts { get; set; } = [];

        [Required]
        public int? CategoryId { get; set; }
        
        public bool IsActive { get; set; }=true;
        [Required]

        public decimal Price { get; set; }
    }
}
