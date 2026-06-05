using LearnStore.Application.Commands.ProductCommands;
using LearnStore.Application.DTO;
using LearnStore.Application.Queries;
using LearnStore.Localization.Resources;
using LearnStore.Web.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Localization;

namespace LearnStore.Web.MVC.Controllers
{
    public class ProductController: Controller
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IStringLocalizer<WebAppResource> _webAppLocalizer;

        public ProductController(IMediator mediator ,IStringLocalizer<SharedResource> SharedLocalizer, IStringLocalizer<WebAppResource> WebAppLocalizer)
        {
            _mediator = mediator;
            _sharedLocalizer = SharedLocalizer;
            _webAppLocalizer = WebAppLocalizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var query = new GetProductsQuery();
            var products = _mediator.Send(query).Result;

            return View(products);
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var query = new GetProductQuery(id);
            var product = _mediator.Send(query).Result;

            if (product is null)
                return NotFound();
            return View(product);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
        public IActionResult AddProduct()
        {
            var productViewModel = new ProductViewModel();
            return View(productViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]

        public async Task<IActionResult> AddProduct(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
                return View(productViewModel);

            var id = GetCurrentUserId();
            var command = new CreateProductCommand()
            {
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Author = new AuthorDto() { Id = productViewModel.AuthorId },
                Category = new ProductCategoryDto() { Id = productViewModel.CategoryId!.Value },
                IsActive = productViewModel.IsActive,
                Price = productViewModel.Price,
                Seller = new SellerDto() { Id = GetCurrentUserId() },
                ChildProducts = productViewModel.ChildProducts.Select(cp => new ProductDto() { Id = cp }).ToList()
            };
            await _mediator.Send(command);

            return RedirectToAction("Products", "Seller");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> EditProduct(int id)
        {
            var query = new GetProductsQuery() { SellerId = GetCurrentUserId() };
            var products = await _mediator.Send(query);
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product is null)
            {
                ModelState.AddModelError(string.Empty, _webAppLocalizer["ProductNotOwnedByUser"]);
                return View();
            }
            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                AuthorId = product.Author!.Id,
                CategoryId = product.Category!.Id,
                IsActive = product.IsActive,
                Price = product.Price,
                ChildProducts = product.ChildProducts.Select(cp => cp.Id).ToList()
            };
            return View(productViewModel);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> EditProduct(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
                return View(productViewModel);
            var query = new GetProductsQuery() { SellerId = GetCurrentUserId() };
            var products = _mediator.Send(query).Result;

            if (!products.Any(p => p.Id == productViewModel.Id))
            {
                ModelState.AddModelError(string.Empty, _webAppLocalizer["ProductNotOwnedByUser"]);
                return View();
            }
            var command = new UpdateProductCommand()
            {
                Id = productViewModel.Id,
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Author = new AuthorDto() { Id = productViewModel.AuthorId },
                Category = new ProductCategoryDto() { Id = productViewModel.CategoryId!.Value },
                IsActive = productViewModel.IsActive,
                Price = productViewModel.Price,
                Seller = new SellerDto() { Id = GetCurrentUserId() },
                ChildProducts = productViewModel.ChildProducts.Select(cp => new ProductDto() { Id = cp }).ToList()
            };

            await _mediator.Send(command);
            return RedirectToAction("Products", "Seller");
        }

        public string GetCurrentUserId()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? string.Empty;
        }
    }
}
