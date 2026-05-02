using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.ValueObjects
{
    public class ProductSearchCriteria
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Author? Author { get; set; }
        public Seller? Seller { get; set; }
        public ProductCategory? Category { get; set; }
        public decimal Price { get; set; }
    }
}
