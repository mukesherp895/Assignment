using Assignment.Common;
using Assignment.Model.DTO;
using Assignment.WebApp.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Assignment.WebApp.Controllers
{
    [CheckSessionAthorization]
    [Route("product")]
    public class ProductController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.PageHeader = "Product";
            return View();
        }
    }
}
