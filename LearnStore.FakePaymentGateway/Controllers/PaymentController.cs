using LearnStore.FakePaymentGateway.Models;
using LearnStore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LearnStore.FakePaymentGateway.Controllers
{
   
    public class PaymentController(ISecretProvider SecretProvider) : Controller
    {
        [HttpGet]
        
        public IActionResult Pay(string orderId, decimal amount, string returnUrl, string notifyUrl)
        {
            var model = new PayViewModel
            {
                OrderId = orderId,
                Amount = amount,
                ReturnUrl = returnUrl,
                NotifyUrl = notifyUrl
            };

            return View(model);
        }

        [HttpPost]
        
        public async Task<IActionResult> Pay(string orderId, decimal amount, string returnUrl, string notifyUrl, string status)
        {
                var transactionId = Guid.NewGuid().ToString();

                var data = $"{orderId}|{transactionId}|{status}|{amount}";

                var secretKey = SecretProvider.GetSecret("FakePaymentGatewayKey");

                using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
                var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                var signature = Convert.ToBase64String(signatureBytes);

                var payload = new
                {
                    OrderId = orderId,
                    TransactionId = transactionId,
                    Status = status,
                    Amount = amount,
                    Signature = signature
                };

                using (var client = new HttpClient())
                {
                    var json = JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    await client.PostAsync(notifyUrl, content);
                }

                var redirectUrl = $"{returnUrl}?orderId={orderId}&transactionId={transactionId}";
                return Redirect(redirectUrl);
        }
    }
}
