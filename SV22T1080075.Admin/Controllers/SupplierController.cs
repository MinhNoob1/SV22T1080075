using Microsoft.AspNetCore.Mvc;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    public class SupplierController : Controller
    {
        private const int PAGE_SIZE = 10;
        public async Task<IActionResult> Index(int page = 1, string searchValue ="")
        {
            var data = await CommonDataServices.SupplierDB.ListAsync(page, PAGE_SIZE, searchValue);
            var rowCount = await CommonDataServices.SupplierDB.CountAsync(searchValue);
            var model = new Models.PaginationSearchResult<Supplier>()
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
            ViewBag.Title = "Bổ sung nhà cung cấp mới";
            var model = new Supplier() 
            {
                SupplierID = 0
            };
            return View("Edit", model);
        }
        public async Task<IActionResult> Edit(int id = 0)
        {
            var model = await CommonDataServices.SupplierDB.GetAsync(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveData(Supplier data)
        {
            try
            {
                ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp mới" : "Cập nhật thông tin nhà cung cấp";
                //Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(data.SupplierName))
                    ModelState.AddModelError("SupplierName", "Tên nhà cung cấp không được để trống");
                if (string.IsNullOrWhiteSpace(data.ContantName))
                    ModelState.AddModelError("ContantName", "Tên liên hệ không được để trống");
                if (string.IsNullOrWhiteSpace(data.Phone))
                    ModelState.AddModelError("Phone", "Số điện thoại không được để trống");
                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError("Email", "Email không được để trống");
                if (string.IsNullOrWhiteSpace(data.Andress))
                    ModelState.AddModelError("Andress", "Địa chỉ không được để trống");
                if (string.IsNullOrWhiteSpace(data.Province))
                    ModelState.AddModelError("Province", "Tỉnh/Thành phố không được để trống");

                //Thông báo lỗi và yêu cầu nhập lại nếu có trường hợp dữ liệu không hợp lệ
                if (!ModelState.IsValid)
                    return View("Edit", data);
                if (data.SupplierID == 0)
                    await CommonDataServices.SupplierDB.AddAsync(data);
                else
                    await CommonDataServices.SupplierDB.UpdateAsync(data);
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("Error", ex.Message);
                return View("Edit", data);
            }
        }
        public async Task<IActionResult> Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                await CommonDataServices.SupplierDB.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            else 
            {
                var model = await CommonDataServices.SupplierDB.GetAsync(id);
                if (model == null)
                    return RedirectToAction("Index");
                return View(model);
            }
        }
        
    }
}