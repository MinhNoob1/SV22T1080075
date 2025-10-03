using Microsoft.AspNetCore.Mvc;

namespace SV22T1080075.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
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