using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1080075.Admin.Models;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string PRODUCT_SEARCH_FOR_SALE = "ProductSearchSale";
        private const string CART = "Cart";
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            return View();
        }
        public IActionResult Create()
        {
            var condition = ApplicationContext.GetSessionData<ProductSearchCondition>(PRODUCT_SEARCH_FOR_SALE);
            if (condition == null)
            {
                condition = new ProductSearchCondition()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    maxPrice = 0,
                    minPrice = 0,
                };
            }
            return View(condition);
        }
        /// <summary>
        /// Tìm kiếm mặt hàng để đưa vào giỏ
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SearchProducts(ProductSearchCondition condition)
        {
            if (condition == null)
            {
                return Content("Yêu cầu không hợp lệ");
            }
            var model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                CategoryID = condition.CategoryID,
                SupplierID = condition.SupplierID,
                MaxPrice = condition.maxPrice,
                MinPrice = condition.minPrice,
                Data = await ProductDataService.ProductDB.ListAsync(
                        condition.Page,
                        condition.PageSize,
                        condition.SearchValue,
                        condition.CategoryID,
                        condition.SupplierID,
                        condition.maxPrice,
                        condition.minPrice
                    ),
                RowCount = await ProductDataService.ProductDB.CountAsync(
                        condition.SearchValue,
                        condition.CategoryID,
                        condition.SupplierID,
                        condition.maxPrice,
                        condition.minPrice
                    )
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_FOR_SALE, condition);
            return View(model);
        }
        /// <summary>
        /// Lấy giỏ hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            var session = GetSessionCart();
            return View(session);
        }
        public IActionResult AddToCart(OrderDetail data)
        {
            AddSessionCart(data);
            var session = GetSessionCart();
            return View("GetCart", session);
        }
        /// <summary>
        /// Lấy giỏ hàng trong có trong session
        /// </summary>
        /// <returns></returns>
        private List<OrderDetail> GetSessionCart() 
        {
            var cart = ApplicationContext.GetSessionData<List<OrderDetail>>(CART);
            if (cart == null)
            {
                cart = new List<OrderDetail>();
            }
            return cart;
        }
        /// <summary>
        /// Thêm hàng vào giỏ trong session
        /// </summary>
        /// <param name="data"></param>
        private void AddSessionCart(OrderDetail data)
        {
            var cart = GetSessionCart();
            var existOrderDetails = cart.Find(m => m.ProductID == data.ProductID);
            if (existOrderDetails == null)
            {
                cart.Add(data);
            }
            else
            {
                existOrderDetails.Quantity += data.Quantity;
                existOrderDetails.SalePrice = data.SalePrice;
            }
            ApplicationContext.SetSessionData(CART, cart);
        }
        public IActionResult Details(int id = 0)
        {
            return View();
        }
        public IActionResult EditDetails(int id = 0)
        {
            return View();
        }
        public IActionResult Shipping()
        {
            return View();
        }
    }
}