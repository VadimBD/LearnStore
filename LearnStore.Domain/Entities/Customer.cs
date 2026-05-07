using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; }= string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<Product> PurchasedProducts { get; set; } = [];
    }
}
