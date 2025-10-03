using Microsoft.AspNetCore.Mvc;

namespace SV22T1080075.Admin.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung khách hàng mới";
            return View("Edit");
        }
        public IActionResult Edit(int id = 0) 
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            return View();     
        }
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}