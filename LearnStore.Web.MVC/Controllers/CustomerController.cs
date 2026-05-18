using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.MVC.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
