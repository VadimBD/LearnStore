using LearnStore.Application.Commands;
using LearnStore.Application.Commands.ProductCommands;
using LearnStore.Application.DTO;
using LearnStore.Application.Queries;
using LearnStore.Domain.Entities;
using LearnStore.Domain.ValueObjects;
using LearnStore.Localization.Resources;
using LearnStore.Web.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

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
        [Authorize]
        public IActionResult Index()
        {
            var query = new GetProductsQuery();
            var products = _mediator.Send(query).Result;

            return View(products);
        }

        [HttpGet]
        public async Task< IActionResult> Details(int id)
        {
            var query = new GetProductQuery(id);
            var product = await _mediator.Send(query);

            if (product is null)
                return NotFound();
            return View(product);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> AddProduct()
        {
            var getAuthorsQuery= new GetAuthorsQuery();
           
            var getProductCategoriesQuery = new GetProductCategoriesQuery();
            var productViewModel = new ProductViewModel()
            {
                Authors = await _mediator.Send(getAuthorsQuery),
                Categories=await _mediator.Send(getProductCategoriesQuery),
                Product=new()
            };
            return View("Edit",productViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]

        public async Task<IActionResult> AddProduct(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
                return View(productViewModel);

            var sellerId = GetCurrentUserId();
            using var stream = productViewModel.File.OpenReadStream();
            var saveFileCommand = new SaveFileCommand()
            {
                FileStream = stream,
                OriginalName = productViewModel.File.FileName,
                SellerId = sellerId,
            };
            
            var savedFileInfo = await _mediator.Send(saveFileCommand);

            var command = new CreateProductCommand()
            {
               Name=productViewModel.Product.Name,
               Description = productViewModel.Product.Description,
               Author=productViewModel.Product.Author,
               Seller=new SellerDto() { Id=sellerId},
               Category=productViewModel.Product.Category,
               ChildProducts=productViewModel.Product.ChildProducts,
               Price=productViewModel.Product.Price,
               FileName=savedFileInfo.OriginalName,
               FileStorageName=savedFileInfo.StorageName,
               IsActive=productViewModel.Product.IsActive,
            };
            await _mediator.Send(command);

            return RedirectToAction("Products", "Seller");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query=new GetProductsQuery() { 
            IsActive=true
            };
            var products = await _mediator.Send(query);
            var vm= new ProductListViewModel(){Products= products};
            return View(vm);
        }

        [HttpGet("{id}")]
        [Authorize]
        [Route("DocumentView")]
        public async Task<IActionResult> DocumentView(int id)
        {
            var product=await _mediator.Send(new GetProductQuery(id));
            var fileStream = await _mediator.Send(new ReadFileQuery(product.FileStorageName, product.Seller.Id));
            return View (fileStream);
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
                //Id = product.Id,
                //Name = product.Name,
                //Description = product.Description,
                //AuthorId = product.Author!.Id,
                //CategoryId = product.Category!.Id,
                //IsActive = product.IsActive,
                //Price = product.Price,
                //ChildProducts = product.ChildProducts.Select(cp => cp.Id).ToList()
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

            if (!products.Any(p => p.Id == productViewModel.Product.Id))
            {
                ModelState.AddModelError(string.Empty, _webAppLocalizer["ProductNotOwnedByUser"]);
                return View();
            }
            var command = new UpdateProductCommand()
            {
                //Id = productViewModel.Id,
                //Name = productViewModel.Name,
                //Description = productViewModel.Description,
                //Author = new AuthorDto() { Id = productViewModel.AuthorId },
                //Category = new ProductCategoryDto() { Id = productViewModel.CategoryId!.Value },
                //IsActive = productViewModel.IsActive,
                //Price = productViewModel.Price,
                //Seller = new SellerDto() { Id = GetCurrentUserId() },
                //ChildProducts = productViewModel.ChildProducts.Select(cp => new ProductDto() { Id = cp }).ToList()
            };

            await _mediator.Send(command);
            return RedirectToAction("Products", "Seller");
        }

        public string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("User ID not found.");
        }
    }
}
