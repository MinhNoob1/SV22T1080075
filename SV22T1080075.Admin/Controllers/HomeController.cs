using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1080075.Admin.Models;
using SV22T1080075.DataLayers;
using System.Diagnostics;

namespace SV22T1080075.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Test()
        {
            string connectionString = "Server=localhost;" +
                                      "Database=LiteCommerce;" +
                                      "Trusted_Connection=True;" +
                                      "MultipleActiveResultSets=true;" +
                                      "TrustServerCertificate=true";
            var dal = new CustomerDAL(connectionString);
            var data = await dal.ListAsync();
            return Json(data.ToList());
        }
    }
}
