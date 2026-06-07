using LearnStore.Application.Commands.PaymentCommands;
using LearnStore.Application.Interfaces;
using LearnStore.Application.Queries;
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
        [HttpPost("/Payment/Webhook")]
        public async Task<IActionResult> Webhook(PaymentNotification dto)
        {
            if (!SignatureValidator.IsValid(dto))
                return BadRequest();
            var command = new CreatePaymentCommand { OrderId = new Guid(dto.OrderId), TransactionId = dto.TransactionId, Status = PaymentStatusMapper.Map(dto.Status), Amount = dto.Amount };
           
            await Mediator.Send(command, CancellationToken.None);
            return Ok();
        }

        [HttpGet("/Payment/Callback")]
        public async Task<IActionResult> Callback(string orderId)
        {
            var getOrderCommand = new GetOrderQuery(new Guid(orderId));
            var order = await Mediator.Send(getOrderCommand, CancellationToken.None);
            if (order == null)
                return View("Result", WebAppLocalizer["OrderNotFound"]);
            var payment = order.Payments.MaxBy(p => p.PaymentDate0);
            if (payment == null)
                return View("Result", WebAppLocalizer["PaymentNotFound"]);

            return View("Result", payment.Status);
        }
    }
}
