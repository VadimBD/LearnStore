using LearnStore.Application.Queries;
using LearnStore.Domain.Entities;
using LearnStore.Web.Api.Models.Customer;
using LearnStore.Web.Api.Models.Seller;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace LearnStore.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController(IMediator Mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CustomerResponse>> Get([FromQuery] GetSellerRequest request)
        {
            var query = new GetCustomerQuery(request.Id);
            var customer = await Mediator.Send(query);
            if (customer is null)
                return NotFound();
            return Ok(customer);
        }
    }
}
