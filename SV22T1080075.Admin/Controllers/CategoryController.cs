using Microsoft.AspNetCore.Mvc;
using SV22T1080075.Admin.Models;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;
using System.Buffers;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const string CATEGORY_SEARCH_CONDITION = "CategorySearchCondition";
        public IActionResult Index()
        {
            // Nếu trong session có lưu điều kiện tìm kiếm thì sử dụng lại điều kiện đó,
            // Ngược lại, thì tạo điều kiện tìm kiếm mặc định
            var conditon = ApplicationContext.GetSessionData<PaginationSearchCondition>(CATEGORY_SEARCH_CONDITION);
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
            var data = await CommonDataServices.CategoryDB.ListAsync(condition.Page, condition.PageSize, condition.SearchValue);
            var rowCount = await CommonDataServices.CategoryDB.CountAsync(condition.SearchValue);
            var model = new PaginationSearchResult<Category>()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            // Lưu lại điều kiện tìm kiếm vào trong session
            ApplicationContext.SetSessionData(CATEGORY_SEARCH_CONDITION, condition);
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
