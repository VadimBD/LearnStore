using LearnStore.Admin.Models;
using LearnStore.Application.Commands.ProductCommands;
using LearnStore.Application.DTO;
using LearnStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ProductsFilter filter)
        {
            var query = new GetProductsQuery()
            {
                AuthorId = filter.AuthorId,
                CategoryId = filter.CategoryId,
                IsActive = filter.IsActive,
                Price = filter.Price,
                ProductName = filter.ProductName,
                SellerId = filter.SellerId
            };
            var products = await _mediator.Send(query);
            return View(products);

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
                return View(model);
            var query = new GetProductsQuery()
            {
                AuthorId = model.AuthorId,
                CategoryId = model.CategoryId,
                IsActive = model.IsActive,
                Price = model.Price,
                ProductName = model.Name,
                SellerId = model.SellerId
            };
            await _mediator.Send(query);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var query = new GetProductQuery(id);
            var product = await _mediator.Send(query);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetProductQuery(id);
            var product = await _mediator.Send(query);
            if (product == null)
                return NotFound();
            var model = new ProductViewModel()
            {
                ProductId = product.Id,
                AuthorId = product.Author.Id,
                CategoryId = product.Category.Id,
                IsActive = product.IsActive,
                Price = product.Price,
                Name = product.Name,
                SellerId = product.Seller.Id
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var query = new GetProductQuery(model.ProductId);
            var product = await _mediator.Send(query);
            if (product == null)
                return NotFound();
            var command = new UpdateProductCommand()
            {
                Id = model.ProductId,
                Author = new AuthorDto { Id = model.AuthorId },
                Category = new ProductCategoryDto { Id = model.CategoryId },
                IsActive = model.IsActive,
                Price = model.Price,
                Name = model.Name,
                Seller = new SellerDto { Id = model.SellerId }
            };
            await _mediator.Send(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteProductCommand(id);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
