using LearnStore.Application.Commands.ProductCommands;
using LearnStore.Application.Queries;
using LearnStore.Web.Api.Models.Product;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IMediator Mediator) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> Get(int id)
        {
            var getProductQuery = new GetProductQuery(id);
            var product = await Mediator.Send(getProductQuery);
            if (product != null)
            {
                return Ok(product.Adapt<ProductResponse>());
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Post([FromBody] CreateProductRequest request)
        {
            var createProductCommand = request.Adapt<CreateProductCommand>();
            await Mediator.Send(createProductCommand);

            return Ok("Product created successfully.");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put([FromBody] UpdateProductRequest request)
        {
            var updateProductCommand = request.Adapt<UpdateProductCommand>();
            await Mediator.Send(updateProductCommand);
            return Ok("Product updated successfully.");
        }
    }
}
