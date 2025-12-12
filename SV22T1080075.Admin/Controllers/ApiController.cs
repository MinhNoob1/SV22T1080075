using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;

namespace SV22T1080075.Admin.Controllers
{
    [Authorize]
    public class ApiController : Controller
    {
        public IActionResult Customer(int id)
        {
            var data = CommonDataServices.CustomerDB.GetAsync(id);
            if (data == null)
            {
                return Json(new Customer());
            }
            return Json(data);
        }
    }
}
