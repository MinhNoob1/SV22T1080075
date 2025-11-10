using Microsoft.AspNetCore.Mvc;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DataLayers;
using SV22T1080075.DomainModels;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private const int PAGE_SIZE = 20;
        public async Task<IActionResult> Index(int page = 1, string searchValue = "")
        {
            var data = await CommonDataServices.CustomerDB.ListAsync(page, PAGE_SIZE, searchValue);
            var rowCount = await CommonDataServices.CustomerDB.CountAsync(searchValue);
            var model = new Models.PaginationSearchResult<Customer>()
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
            ViewBag.Title = "Bổ sung khách hàng mới";
            var model = new Customer()
            {
                CustomerID = 0
            };
            return View("Edit", model);
        }
        public async Task<IActionResult> Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            var model = await CommonDataServices.CustomerDB.GetAsync(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        /// <summary>
        /// Lưu dữ liệu khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveData(Customer data)
        {
            try
            {
                ViewBag.Title = data.CustomerID == 0 ? "Bổ sung khách hàng mới" : "Cập nhật thông tin khách hàng";

                //Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(data.CustomerName))
                    ModelState.AddModelError("CustomerName", "Tên khách hàng không được để trống");
                if (string.IsNullOrWhiteSpace(data.ContactName))
                    ModelState.AddModelError("ContactName", "Tên liên hệ không được để trống");
                if (string.IsNullOrWhiteSpace(data.Phone))
                    ModelState.AddModelError("Phone", "Điện thoại không được để trống");
                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError("Email", "Email không được để trống");
                if (string.IsNullOrWhiteSpace(data.Address))
                    ModelState.AddModelError("Address", "Địa chỉ không được để trống");
                if (string.IsNullOrWhiteSpace(data.Province))
                    ModelState.AddModelError("Province", "Tỉnh/Thành phố không được để trống");

                //Thông báo lỗi và yêu cầu nhập lại nếu có trường hợp dữ liệu không hợp lệ
                if (!ModelState.IsValid)
                    return View("Edit", data);

                if (data.CustomerID == 0)
                {
                    await CommonDataServices.CustomerDB.AddAsync(data);
                }
                else
                {
                    await CommonDataServices.CustomerDB.UpdateAsync(data);
                }
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
                await CommonDataServices.CustomerDB.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            else
            {
                var model = await CommonDataServices.CustomerDB.GetAsync(id);
                if (model == null)
                    return RedirectToAction("Index");
                return View(model);
            }
        }
    }
}