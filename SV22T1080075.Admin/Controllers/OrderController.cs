using Microsoft.AspNetCore.Mvc;

namespace SV22T1080075.Admin.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Details(int id = 0)
        {
            return View();
        }
        public IActionResult EditDetails(int id = 0)
        {
            return View();
        }
        public IActionResult Shipping()
        {
            return View();
        }
    }
}