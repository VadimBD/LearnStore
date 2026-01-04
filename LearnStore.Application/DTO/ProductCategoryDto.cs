using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.DTO
{
    public record class ProductCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
