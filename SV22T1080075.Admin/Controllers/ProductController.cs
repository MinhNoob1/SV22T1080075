using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SV22T1080075.Admin.Models;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;
using System.Reflection;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";

        public IActionResult Index()
        {
            // Nếu trong session có lưu điều kiện tìm kiếm thì sử dụng lại điều kiện đó,
            // Ngược lại, thì tạo điều kiện tìm kiếm mặc định
            var condition = ApplicationContext.GetSessionData<ProductSearchCondition>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new ProductSearchCondition()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    minPrice = 0,
                    maxPrice = 0
                };
            }
            return View(condition);
        }
        public async Task<IActionResult> Search(ProductSearchCondition condition)
        {
            var data = await ProductDataService.ProductDB.ListAsync(
                condition.Page,
                condition.PageSize,
                condition.SearchValue,
                condition.CategoryID,
                condition.SupplierID,
                condition.minPrice,
                condition.maxPrice
            );
            var rowCount = await ProductDataService.ProductDB.CountAsync(
                condition.SearchValue,
                condition.CategoryID,
                condition.SupplierID,
                condition.minPrice,
                condition.maxPrice
            );
            var result = new PaginationSearchResult<Product>()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            // Lưu lại điều kiện tìm kiếm vào trong session
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);
            return View(result);
        }
        /// <summary>
        /// Xử lý các thuộc tính liên quan đến ProductAttribute
        /// </summary>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Attribute(int id = 0, string method = "", long attributeId = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");
            var product = await ProductDataService.ProductDB.GetAsync(id);
            if (product == null)
                return RedirectToAction("Index");
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    return View("AttributeEdit", new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = id
                    });
                case "edit":
                    var attribute = await ProductDataService.ProductDB.GetAttributeAsync(attributeId);
                    if (attribute == null)
                        return RedirectToAction("Edit", new { id = id });
                    ViewBag.Title = "Cập nhật thuộc tính mặt hàng";
                    return View("AttributeEdit", attribute);
                case "delete":
                    await ProductDataService.ProductDB.DeleteAttributeAsync(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Edit", new { id = id });
            }
        }
        /// <summary>
        /// Xử lý các thuộc tính ảnh liên quan đến ProductPhoto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Photo(int id = 0, string method = "", long photoId = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");
            var product = await ProductDataService.ProductDB.GetAsync(id);
            if (product == null)
                return RedirectToAction("Index");
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View("PhotoEdit", new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = id
                    });
                case "edit":
                    var photo = await ProductDataService.ProductDB.GetPhotoAsync(photoId);
                    if (photo == null)
                        return RedirectToAction("Edit", new { id = id });
                    ViewBag.Title = "Cập nhật hình ảnh mặt hàng";
                    return View("PhotoEdit", photo);
                case "delete":
                    await ProductDataService.ProductDB.DeletePhotoAsync(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Edit", new { id = id });
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveData(ProductEditModel model)
        {
            ViewBag.Title = model.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật mặt hàng";
            // Validate
            if (string.IsNullOrWhiteSpace(model.ProductName))
                ModelState.AddModelError("ProductName", "Tên mặt hàng không được để trống");
            if (!ModelState.IsValid)
            {
                // BẮT BUỘC load lại dropdown + dữ liệu phụ    
                ViewBag.Categories = new SelectList(ProductDataService.ListCategories(), "CategoryID", "CategoryName");
                ViewBag.Suppliers = new SelectList(ProductDataService.ListSuppliers(), "SupplierID", "SupplierName");
                ViewBag.Photos = ProductDataService.ListPhotos(model.ProductID);
                ViewBag.Attributes = ProductDataService.ListAttributes(model.ProductID);
                return View("Edit", model);
            }
            // Upload ảnh nếu có
            if (model.UpLoadFile != null)
            {
                string fileName = $"{model.UpLoadFile.FileName}";
                string filePath = Path.Combine(ApplicationContext.WWWRootPath, @"images\products", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.UpLoadFile.CopyToAsync(stream);
                }
                model.Photo = fileName;
            }
            // Convert sang Product để lưu
            Product data = new Product()
            {
                ProductID = model.ProductID,
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                SupplierID = model.SupplierID,
                CategoryID = model.CategoryID,
                Unit = model.Unit,
                Price = model.Price,
                Photo = model.Photo,
                IsSelling = model.IsSelling
            };
            if (data.ProductID == 0)
                await ProductDataService.ProductDB.AddAsync(data);
            else
                await ProductDataService.ProductDB.UpdateAsync(data);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Cập nhật thông tin mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            var product = await ProductDataService.ProductDB.GetAsync(id);
            if (product == null)
                return RedirectToAction("Index");
            // Tạo model Edit từ Product
            var model = new ProductEditModel()
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                SupplierID = product.SupplierID,
                CategoryID = product.CategoryID,
                Unit = product.Unit,
                Price = product.Price,
                Photo = product.Photo,
                IsSelling = product.IsSelling
            };
            // Dropdown
            ViewBag.Categories = new SelectList(ProductDataService.ListCategories(), "CategoryID", "CategoryName");
            ViewBag.Suppliers = new SelectList(ProductDataService.ListSuppliers(), "SupplierID", "SupplierName");
            // Ảnh + thuộc tính
            ViewBag.Photos = ProductDataService.ListPhotos(id);
            ViewBag.Attributes = ProductDataService.ListAttributes(id);
            return View(model);
        }
        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id = 0)
        {
            if (id <= 0)
                return RedirectToAction("Index");
            var data = await ProductDataService.ProductDB.GetAsync(id);
            if (data == null)
                return RedirectToAction("Index");
            if (await ProductDataService.ProductDB.InUsedAsync(id))
                return RedirectToAction("Index");
            return View(data);
        }
        /// <summary>
        /// Xác nhận xóa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await ProductDataService.ProductDB.InUsedAsync(id))
                return RedirectToAction("Index");
            await ProductDataService.ProductDB.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}