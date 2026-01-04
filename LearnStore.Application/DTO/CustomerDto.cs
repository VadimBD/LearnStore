using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.DTO
{
    public record class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
