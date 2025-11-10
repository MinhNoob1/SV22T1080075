using Microsoft.AspNetCore.Mvc;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 10;
        public async Task<IActionResult> Index(int page = 1, string searchValue = "")
        {
            var data = await CommonDataServices.ShipperDB.ListAsync(page, PAGE_SIZE, searchValue);
            var rowCount = await CommonDataServices.ShipperDB.CountAsync(searchValue);
            var model = new Models.PaginationSearchResult<SV22T1080075.DomainModels.Shipper>()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data
            };
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung người giao hàng mới";
            var model = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit", model);
        }
        public async Task<IActionResult> Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin người giao hàng";
            var model = await CommonDataServices.ShipperDB.GetAsync(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveData(Shipper data)
        {
            try
            {
                ViewBag.Title = data.ShipperID == 0 ? "Bổ sung người giao hàng mới" : "Cập nhật thông tin người giao hàng";

                //Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(data.ShipperName))
                    ModelState.AddModelError("ShipperName", "Tên người giao hàng không được để trống");
                if (string.IsNullOrWhiteSpace(data.Phone))
                    ModelState.AddModelError("Phone", "Điện thoại không được để trống");

                //Thông báo lỗi và yêu cầu nhập lại nếu có trường hợp dữ liệu không hợp lệ
                if (!ModelState.IsValid)
                    return View("Edit", data);
                if (data.ShipperID == 0)
                    await CommonDataServices.ShipperDB.AddAsync(data);
                else
                    await CommonDataServices.ShipperDB.UpdateAsync(data);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View("Edit", data);
            }
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataServices.ShipperDB.DeleteAsync(id).Wait();
                return RedirectToAction("Index");
            }
            else
            {
                var model = CommonDataServices.ShipperDB.GetAsync(id).Result;
                if (model == null)
                    return RedirectToAction("Index");
                return View(model);
            }
        }
    }
}