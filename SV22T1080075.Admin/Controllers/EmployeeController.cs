using Microsoft.AspNetCore.Mvc;

namespace SV22T1080075.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
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