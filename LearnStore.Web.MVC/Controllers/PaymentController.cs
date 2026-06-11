using LearnStore.Application.Commands.CustomerCommands;
using LearnStore.Application.Commands.OrderCommands;
using LearnStore.Application.Commands.PaymentCommands;
using LearnStore.Application.Commands.SellerCommands;
using LearnStore.Application.DTO;
using LearnStore.Application.Interfaces;
using LearnStore.Application.Queries;
using LearnStore.Domain.Entities;
using LearnStore.Domain.Enums;
using LearnStore.Localization.Resources;
using LearnStore.Web.MVC.interfaces;
using LearnStore.Web.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace LearnStore.Web.MVC.Controllers
{
    
    public class PaymentController(ISignatureValidator SignatureValidator,IMediator Mediator, IStringLocalizer<WebAppResource> WebAppLocalizer, IPaymentStatusMapper PaymentStatusMapper) : Controller
    {
        private const decimal SellerInterest = 0.9m; 
        [HttpPost("/Payment/Webhook")]
        public async Task<IActionResult> Webhook([FromBody] PaymentNotification dto)
        {
            if (!SignatureValidator.IsValid(dto))
                return BadRequest();
            var paymentStatus = PaymentStatusMapper.Map(dto.Status);
            var orderId = new Guid(dto.OrderId);
            var command = new CreatePaymentCommand { OrderId = orderId , TransactionId = dto.TransactionId, Status = paymentStatus, Amount = dto.Amount };
           
            await Mediator.Send(command, CancellationToken.None);

            if (paymentStatus == PaymentStatus.Success) 
            {
                var products = await Mediator.Send(new GetOrderProductsQuery() { Orderid = orderId });
                
                foreach (var item in products) {
                    await Mediator.Send(new AddToSellerBalanceCommand(){SellerId= item.Seller.Id , Ammaunt= item.Price * SellerInterest });
                }
                await Mediator.Send(new AddOrderProductsToCustomerCommand() { OrderId = orderId });
                await Mediator.Send(new UpdateOrderStateCommand() { OrderId = orderId, OrderState = OrderState.Completed });
            }
            else
                await Mediator.Send(new UpdateOrderStateCommand() { OrderId = orderId, OrderState = OrderState.Pending });

            return Ok();
        }

        [HttpGet("/Payment/Callback")]
        public async Task<IActionResult> Callback([FromQuery]string orderId)
        {
            var getOrderCommand = new GetOrderQuery(new Guid(orderId));
            var order = await Mediator.Send(getOrderCommand, CancellationToken.None);
            if (order == null)
                return View("Result", WebAppLocalizer["OrderNotFound"]);
            var payment = order.Payments.MaxBy(p => p.PaymentDate);
            if (payment == null)
                return View("Result", WebAppLocalizer["PaymentNotFound"]);

            if (payment.Status == PaymentStatus.Success) {
              
                return RedirectToAction("Complete", "Order");
            }
           
            else
                return View("Result", WebAppLocalizer["PaymentFailed"]);

           
        }
    }
}
