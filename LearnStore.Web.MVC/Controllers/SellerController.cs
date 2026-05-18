using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.MVC.Controllers
{
    public class SellerController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
