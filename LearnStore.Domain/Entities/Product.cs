using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Entities
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; }=string.Empty;
        
        public Author? Author { get; set; }
        public Seller?  Seller { get; set; }
        public ICollection<Product> ChildProducts { get; set; }= [];
        public string FileName { get; set; }       
        public string FileStorageName { get; set; }  

        public ProductCategory? Category { get; set; }

        public bool IsActive { get; set; }
        public decimal Price { get; set; }

    }
}
