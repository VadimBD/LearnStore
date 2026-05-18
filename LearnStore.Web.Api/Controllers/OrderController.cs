using LearnStore.Application.Commands.OrderCommands;
using LearnStore.Application.Queries;
using LearnStore.Web.Api.Models.Order;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IMediator Mediator) : ControllerBase
    {
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<OrderResponse>> Get(Guid id)
        {
            var query = new GetOrderQuery(id);
            var order = await Mediator.Send(query);
            return order != null ? Ok(order.Adapt<OrderResponse>()) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> Post([FromBody] CreateOrderRequest request)
        {
            var command = request.Adapt<CreateOrderCommand>();
            var order = await Mediator.Send(command);
            return Ok("Order created successfully!");
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<OrderResponse>> Put([FromBody] UpdateOrderRequest request)
        {
            var command = request.Adapt<UpdateOrderCommand>();
            await Mediator.Send(command);
            return Ok("Order updated successfully!");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete([FromQuery] DeleteOrderRequest request)
        {
            var command = request.Adapt<DeleteOrderCommand>();
            var result = await Mediator.Send(command);

            if (result == null)
                return NotFound("Order not found");

            return Ok("Order deleted successfully!");
        }
    }
}
