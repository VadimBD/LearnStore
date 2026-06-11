using LearnStore.Application.Commands.OrderCommands;
using LearnStore.Application.DTO;
using LearnStore.Application.Queries;
using LearnStore.Domain.Entities;
using LearnStore.Web.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Identity.Client;
using System;
using System.Security.Claims;

namespace LearnStore.Web.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Cart cart;
        private readonly IConfiguration configuration;
        public OrderController(IMediator mediator, Cart cart, IConfiguration configuration)
        {
            _mediator = mediator;
            this.cart = cart;
            this.configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder()
        {
            var createOrderModel = new CreateOrderViewModel()
            {
                Items = cart.Lines,
                TotalAmount= await _mediator.Send(new CalculateTotalQuery(cart.Lines.Select(l => new CalculateTotalItem(l.Product.Id, 1)).ToList()))
            };
            return View(createOrderModel);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SubmitOrder()
        {
            var id = GetCurrentUserId();
            var calculateTotalQuery = new CalculateTotalQuery(cart.Lines.Select(i => new CalculateTotalItem(i.Product.Id, 1)).ToList());
            var totalAmount = await _mediator.Send(calculateTotalQuery);

            var query = new CreateOrderCommand()
            {
                Customer = new CustomerDto() { Id = id },
                Items = cart.Lines.Select(l => new OrderItemDto()
                {
                    Product = new ProductDto() { Id = l.Product.Id },
                    Quantity = 1
                }).ToList()
            };
            var order = await _mediator.Send(query);
           
            var paymentUrl = $"{configuration["Payment:Url"]}";

            var queryParams = new Dictionary<string, string?>()
            {
                { "orderId", order.Id.ToString() },
                { "amount", totalAmount.ToString() },
                { "returnUrl", GetReturnUrl("Payment/Callback") },
                {"notifyUrl",GetReturnUrl("Payment/Webhook") }
            };
            var redirectUrl = QueryHelpers.AddQueryString(paymentUrl, queryParams);

            return Redirect(redirectUrl);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Orders()
        {
            var id = GetCurrentUserId();
            var query = new GetOrdersQuery() { CustomerId = id };
            var orders = await _mediator.Send(query);
            return View(orders);
        }
        [HttpGet]
        public IActionResult Complete()
        {
            return View();
        }
        public string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("User ID not found.");
        }

        private string GetReturnUrl(string path)
        {
            var redirectUrlBilder = new UriBuilder()
            {
                Scheme = Request.Scheme,
                Host = Request.Host.Host,
                Port = Request.Host.Port ?? 80,
                Path = path
            };
            return redirectUrlBilder.ToString();
        }


        
    }
}
