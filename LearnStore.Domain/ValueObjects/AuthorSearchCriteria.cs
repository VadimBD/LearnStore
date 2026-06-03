using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.ValueObjects
{
    public class AuthorSearchCriteria
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
    }
}
