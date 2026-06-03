using LearnStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.MVC.Controllers
{
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Index()
        {
            var query = new GetCustomerQuery(GetCurrentUserId());
            var customer = _mediator.Send(query).Result;
            return View(customer);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("Orders")]
        public IActionResult GetOrders()
        {
            var query = new GetOrdersQuery() { CustomerId = GetCurrentUserId() };
            var orders = _mediator.Send(query).Result;
            return View(orders);
        }
        public string GetCurrentUserId()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? string.Empty;
        }
    }
}
