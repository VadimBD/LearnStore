using LearnStore.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Commands.AuthCommands
{
    public class LoginCommand:IRequest<LoginResponse>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = [];
        public string Password { get; set; } = string.Empty;
    }
}
