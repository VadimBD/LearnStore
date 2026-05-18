using LearnStore.Application.DTO;
using LearnStore.Application.Queries;
using LearnStore.Web.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.MVC.Controllers
{
    [Route("[controller]")]
    public class SellerController : Controller
    {
        private readonly IMediator _mediator;

        public SellerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var query= new GetSellerQuery(id);
            var seller = await _mediator.Send(query);
            if(seller == null)
            {
                return NotFound();
            }
            var productsQuery = new GetProductsQuery() { SellerId = id };
            var products = await _mediator.Send(productsQuery);

            var sellerViewModel = new SellerViewModel()
            {
                Seller= seller,
                Products= products,
            };
            return View(sellerViewModel);
        }
    }
}
