using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.DTO
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = [];
    }
}
