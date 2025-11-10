using Microsoft.AspNetCore.Mvc;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 10;
        public async Task<IActionResult> Index(int page = 1, string searchValue = "")
        {
            var data = await CommonDataServices.CategoryDB.ListAsync(page, PAGE_SIZE, searchValue);
            var rowCount = await CommonDataServices.CategoryDB.CountAsync(searchValue);
            var model = new Models.PaginationSearchResult<Category>()
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
            var model = new Category()
            {
                CategoryID = 0
            };
            return View("Edit", model);
        }
        public async Task<IActionResult> Edit(int id = 0) 
        {
            var model = await CommonDataServices.CategoryDB.GetAsync(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        public async Task<IActionResult> SaveData(Category data)
        {
            try
            {
                //Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(data.CategoryName))
                    ModelState.AddModelError("CategoryName", "Tên loại hàng không được để trống");
                if (!ModelState.IsValid)
                    return View("Edit", data);
                if (data.CategoryID == 0)
                {
                    await CommonDataServices.CategoryDB.AddAsync(data);
                }
                else
                {
                    await CommonDataServices.CategoryDB.UpdateAsync(data);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Edit", data);
            }
        }
        public async Task<IActionResult> Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                await CommonDataServices.CategoryDB.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            else
            {
                var model = CommonDataServices.CategoryDB.GetAsync(id).Result;
                if (model == null)
                    return RedirectToAction("Index");
                return View(model);
            }
        }
    }
}
