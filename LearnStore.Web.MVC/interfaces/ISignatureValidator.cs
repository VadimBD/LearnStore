using LearnStore.Web.MVC.Models;

namespace LearnStore.Web.MVC.interfaces
{
    public interface ISignatureValidator
    {
        bool IsValid(PaymentNotification dto);
    }
}
