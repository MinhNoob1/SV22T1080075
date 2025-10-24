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
            return View("Edit",model);
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
            //TODO: Kiểm tra tính đúng đắn của dữ liệu đầu vào, kiểm soát lỗi
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