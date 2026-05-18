using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.MVC.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
