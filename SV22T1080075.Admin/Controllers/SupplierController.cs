using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1080075.Admin.Models;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const string SUPPLIER_SEARCH_CONDITION = "SupplierSearchConditon";
        public IActionResult Index()
        {
            // Nếu trong session có lưu điều kiện tìm kiếm thì sử dụng lại điều kiện đó,
            // Ngược lại, thì tạo điều kiện tìm kiếm mặc định
            var conditon = ApplicationContext.GetSessionData<PaginationSearchCondition>(SUPPLIER_SEARCH_CONDITION);
            if (conditon == null)
            {
                conditon = new PaginationSearchCondition()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(conditon);
        }

        public async Task<IActionResult> Search(PaginationSearchCondition condition)
        {
            var data = await CommonDataServices.SupplierDB.ListAsync(condition.Page, condition.PageSize, condition.SearchValue);
            var rowCount = await CommonDataServices.SupplierDB.CountAsync(condition.SearchValue);
            var model = new PaginationSearchResult<Supplier>()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            // Lưu lại điều kiện tìm kiếm vào trong session
            ApplicationContext.SetSessionData(SUPPLIER_SEARCH_CONDITION, condition);
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

                /*
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
                */

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