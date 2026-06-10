using LearnStore.Application.Queries;
using LearnStore.Domain.Entities;
using LearnStore.Domain.Interfaces;
using LearnStore.Web.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.MVC.Controllers
{
    public class CartController : Controller
    {
        
        private Cart cart;
        private IMediator mediator;
        public CartController( Cart cartService,IMediator mediator)
        {
          
            cart = cartService;
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("Index")]
        public IActionResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int Id, string returnUrl)
        {
            var query = new GetProductQuery(Id);
            var product = await mediator.Send(query);
            if (product != null)
            {
                cart.AddItem(product);
            }
            return LocalRedirect(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCartAsync(int productId, string returnUrl)
        {
                cart.RemoveLine(productId);

            return LocalRedirect(returnUrl);
        }
    }
}
