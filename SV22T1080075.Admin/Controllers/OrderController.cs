using Microsoft.AspNetCore.Mvc;
using SV22T1080075.BusinessLayers;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    public class OrderController : Controller
    {
        private const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
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