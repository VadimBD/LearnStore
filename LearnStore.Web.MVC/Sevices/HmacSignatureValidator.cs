using LearnStore.Infrastructure.Interfaces;
using LearnStore.Web.MVC.interfaces;
using LearnStore.Web.MVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace LearnStore.Web.MVC.Sevices
{
    public class HmacSignatureValidator : ISignatureValidator
    {
        private readonly string _secretKey;

        public HmacSignatureValidator(ISecretProvider secretProvider)
        {
            _secretKey = secretProvider.GetSecret("FakePaymentGatewayKey");
        }

        public bool IsValid(PaymentNotification dto)
        {
            var data = $"{dto.OrderId}|{dto.TransactionId}|{dto.Status}|{dto.Amount}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            var computedSignature = Convert.ToBase64String(hash);

            return computedSignature == dto.Signature;
        }
    }
}
