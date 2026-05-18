using LearnStore.Application.Commands.AuthorCommands;
using LearnStore.Web.Api.Models.Author;
using LearnStore.Web.Api.Models.Order;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController(IMediator Mediator) : ControllerBase
    {
        
        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<AuthorResponse>> Post([FromBody] CreateAuthorRequest request)
        {
            var command = request.Adapt<CreateAuthorCommand>();
            var author = await Mediator.Send(command);
            return Ok("Author created successfully!");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuthorResponse>> Put([FromBody] UpdateAuthorRequest request)
        {
            var command = request.Adapt<UpdateAuthorCommand>();
            await Mediator.Send(command);
            return Ok("Author updated successfully!");
        }

    }
}
