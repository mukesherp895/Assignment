using Microsoft.AspNetCore.Mvc;

namespace Assignment.WebApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
