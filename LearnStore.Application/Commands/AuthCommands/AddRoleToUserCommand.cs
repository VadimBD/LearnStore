using LearnStore.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Commands.AuthCommands
{
    public class AddRoleToUserCommand:IRequest<RoleOperationResult>
    {
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
