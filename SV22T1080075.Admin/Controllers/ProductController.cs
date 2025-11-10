using Microsoft.AspNetCore.Mvc;
using SV22T1080075.BusinessLayers;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        public async Task<IActionResult> Index(int page = 1, string searchValue = "")
        {
            var data = await CommonDataServices.ProductDB.ListAsync(page, PAGE_SIZE, searchValue);
            var rowCount = await CommonDataServices.ProductDB.CountAsync(searchValue);
            var model = new Models.PaginationSearchResult<DomainModels.Product>()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data
            };
            return View(model);
        }
        public IActionResult Attribute(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Cập nhật thuộc tính mặt hàng";
                    return View();
                case "delete":
                    //TODO: Xóa xong về lại trang
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Cập nhật hình ảnh mặt hàng";
                    return View();
                case "delete":
                    //TODO: Xóa xong về lại trang
                    return RedirectToAction("Edit", new {id = id});
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult Edit(int id = 0)
        {
            return View();
        }
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}