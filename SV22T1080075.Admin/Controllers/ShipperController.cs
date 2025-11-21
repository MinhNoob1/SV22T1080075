using Microsoft.AspNetCore.Mvc;
using SV22T1080075.Admin.Models;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const string SHIPPER_SEARCH_CONDITION = "ShipperSearchCondition";
        public IActionResult Index()
        {
            // Nếu trong session có lưu điều kiện tìm kiếm thì sử dụng lại điều kiện đó,
            // Ngược lại, thì tạo điều kiện tìm kiếm mặc định
            var conditon = ApplicationContext.GetSessionData<PaginationSearchCondition>(SHIPPER_SEARCH_CONDITION);
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
            var data = await CommonDataServices.ShipperDB.ListAsync(condition.Page, condition.PageSize, condition.SearchValue);
            var rowCount = await CommonDataServices.ShipperDB.CountAsync(condition.SearchValue);
            var model = new PaginationSearchResult<Shipper>()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            // Lưu lại điều kiện tìm kiếm vào trong session
            ApplicationContext.SetSessionData(SHIPPER_SEARCH_CONDITION, condition);
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