using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.DTO
{
    public record class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public AuthorDto? Author { get; set; }
        public SellerDto? Seller { get; set; }
        public ICollection<ProductDto> ChildProducts { get; set; } = [];
        public string? FileName { get; set; }
        public string? FileStorageName { get; set; } 
        public ProductCategoryDto? Category { get; set; }

        public bool IsActive { get; set; }
        public decimal Price { get; set; }

    }
}
