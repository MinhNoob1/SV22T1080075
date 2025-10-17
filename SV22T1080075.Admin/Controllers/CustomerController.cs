using Microsoft.AspNetCore.Mvc;
using SV22T1080075.DataLayers;

namespace SV22T1080075.Admin.Controllers
{
    public class CustomerController : Controller
    {
        public async Task<IActionResult> Index()
        {
            string connectionString = "Server=localhost;" +
                                      "Database=LiteCommerce;" +
                                      "Trusted_Connection=True;" +
                                      "MultipleActiveResultSets=true;" +
                                      "TrustServerCertificate=true";
            var dal = new CustomerDAL(connectionString);
            var data = await dal.ListAsync();

            return View(data);
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