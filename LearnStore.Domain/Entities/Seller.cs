using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Entities
{
    public class Seller
    {
        public string Id { get; set; }= string.Empty;
        public string Name { get; set; }=string.Empty;
        public string EmailAddress { get; set; }= string.Empty;
        public string PhoneNumber { get; set; }= string.Empty;
        public decimal AccountBalance { get; set; } = decimal.Zero;
    }
}
