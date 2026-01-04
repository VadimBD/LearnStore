using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Entities
{
    public class Author
    {
        
        public int Id { get; set; }
        public string FirstName  { get; set; }=string.Empty;
        public string LastName  { get; set; }=string.Empty;
        public string MiddleName  { get; set; }=string.Empty;
        public string Info { get; set; }=string.Empty;
    }
}
