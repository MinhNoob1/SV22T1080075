using Microsoft.AspNetCore.Mvc;

namespace SV22T1080075.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Attribute()
        {
            return View();
        }
        public IActionResult Photo()
        {
            return View();
        }
        public IActionResult Edit(int id = 0)
        {
            return View();
        }
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}