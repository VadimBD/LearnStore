using LearnStore.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Commands.AuthCommands
{
    public class RegisterCommand:IRequest<RegisterResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ICollection<string> Roles { get; set; } = [];
    }
}
