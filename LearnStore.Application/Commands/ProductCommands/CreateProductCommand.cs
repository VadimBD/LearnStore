using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
namespace LearnStore.Application.Commands.ProductCommands
{
    public record class CreateProductCommand:IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AuthorDto? Author { get; set; }
        public SellerDto? Seller { get; set; }
        public ICollection<ProductDto> ChildProducts { get; set; } = [];
        public ProductCategoryDto? Category { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }

    }
}
