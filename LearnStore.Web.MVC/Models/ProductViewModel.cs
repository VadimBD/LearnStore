using LearnStore.Application.DTO;
using LearnStore.Application.UseCases;
using LearnStore.Domain.Entities;
using LearnStore.Localization.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace LearnStore.Web.MVC.Models
{
    public class ProductViewModel
    {

        public ProductDto Product { get; set; }

        public IEnumerable<AuthorDto> Authors { get; set; } = [];
        public IEnumerable<SelectListItem> AuthorsSLI { get => Authors.Select(a => new SelectListItem ($"{a.FirstName} {a.MiddleName} {a.LastName}" , a.Id.ToString()) ) ; }
        public IEnumerable<ProductCategoryDto> Categories { get; set; } = [];
        public IEnumerable<SelectListItem> CategoriesSlI { get => Categories.Select(c => new SelectListItem(c.Name, c.Id.ToString())); }

        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "UploadFileRequired")]
        public IFormFile File { get; set; }
    }
}
