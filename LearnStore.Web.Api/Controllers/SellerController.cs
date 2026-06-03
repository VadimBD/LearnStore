using LearnStore.Application.Commands.SellerCommands;
using LearnStore.Application.UseCases;
using LearnStore.Web.Api.Models.Order;
using LearnStore.Web.Api.Models.Seller;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController(IMediator Mediator) : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<SellerResponse>> Post([FromBody] CreateSellerRequest request)
        {
            var command = request.Adapt<CreateSellerCommand>();
            var seller = await Mediator.Send(command);
            return Ok("Seller created successfully!");
        }
        [HttpPut]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<SellerResponse>> Put([FromBody] UpdateSellerRequest request)
        {
            var command = request.Adapt<UpdateSellerCommand>();
            await Mediator.Send(command);
            return Ok("Seller updated successfully!");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<SellerResponse>> Get([FromQuery] GetSellerRequest request)
        {
            var query = request.Adapt<GetSellerHandler>();
            var seller = await Mediator.Send(query);
            if (seller is null)
                return NotFound();
            return Ok(seller);
        }
    }
}
