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
        /// <summary>
        /// Thêm 1 mặt hàng vào giỏ
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IActionResult AddToCart(OrderDetail data)
        {
            // Kiểm tra tính hợp lệ
            if (data.Quantity < 1)
            {
                return Json(new ApiResult() { Code = 0, Message = "Số lượng không hợp lệ" });
            }
            else if (data.SalePrice < 0)
            {
                return Json(new ApiResult() { Code = 0, Message = "Giá bán không hợp lệ" });
            }
            AddSessionCart(data);
            return Json(new ApiResult() { Code = 1 });
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
        /// <summary>
        /// Xóa 1 mặt hàng trong giỏ hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            try
            {
            var cart = GetSessionCart();
            int index = cart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
            {
                cart.RemoveAt(index);
                ApplicationContext.SetSessionData(CART, cart);
                return Json(new ApiResult() { Code = 1 });
            }
            return Json(new ApiResult() { Code = 0, Message = "Mặt hàng không tồn tại" });
            }
            catch (Exception ex)
            {
                return Json(new ApiResult() { Code = 0, Message= ex.Message });
            }

        }
        /// <summary>
        /// Xóa hết giỏ hàng
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ClearCart()
        {
            try
            {
                var cart = GetSessionCart();
                cart.Clear();
                ApplicationContext.SetSessionData(CART, cart);
                return Json(new ApiResult() { Code = 1 });
            }
            catch (Exception ex)
            {
                return Json(new ApiResult() { Code = 0, Message = ex.Message });
            }
        }
        /// <summary>
        /// Tăng giảm số lượng mặt hàng trong giỏ hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateCartQuantity(int id, int quantity)
        {
            try
            {
                var cart = GetSessionCart();
                var existsProduct = cart.Find(m => m.ProductID == id);
                if (existsProduct != null)
                {
                    int remainQuantity = existsProduct.Quantity + quantity;
                    if (remainQuantity <= 0)
                    {
                        return Json(new ApiResult() { Code = 0, Message = "Số lượng còn lại không hợp lệ" });
                    }
                    existsProduct.Quantity = remainQuantity;
                    ApplicationContext.SetSessionData(CART, cart);
                    return Json(new ApiResult() { Code = 1 });
                }
                return Json(new ApiResult() { Code = 0, Message = "Mặt hàng không tồn tại"});

            }
            catch (Exception ex)
            {
                return Json(new ApiResult() { Code = 0, Message = ex.Message });
            }
        }
        /// <summary>
        /// Lập đơn hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="deliveryProvince"></param>
        /// <param name="deliveryAddress"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Init(string customerID, string deliveryProvince, string deliveryAddress)
        {
            try
            {
                int OrderID = 0;
                var cart = GetSessionCart();
                if (cart.Count == 0)
                {
                    return Json(new ApiResult() { Code = 0, Message = "Không thể lập đơn hàng vì giỏ hàng đang trống"});
                }
                else if (string.IsNullOrWhiteSpace(customerID))
                {
                    return Json(new ApiResult() { Code = 0, Message = "Vui lòng chọn khách hàng" });
                }
                else if (string.IsNullOrWhiteSpace(deliveryProvince))
                {
                    return Json(new ApiResult() { Code = 0, Message = "Vui lòng cho biết tỉnh/thành giao hàng" });
                }
                else if (string.IsNullOrWhiteSpace(deliveryAddress))
                {
                    return Json(new ApiResult() { Code = 0, Message = "Vui lòng nhập địa chỉ giao hàng" });
                }

                Order Data = new Order()
                {
                    CustomerID = Convert.ToInt32(customerID),
                    DeliveryProvince = deliveryProvince,
                    DeliveryAddress = deliveryAddress,
                    EmployeeID = Convert.ToInt32(User.GetUserData()?.UserId),
                    Status = Constants.ORDER_INIT,

                };
                OrderID = await OrderDataService.OrderDB.AddAsync(Data);
                foreach (var item in cart)
                {
                    await OrderDataService.OrderDB.SaveDetailAsync(OrderID, item.ProductID, item.Quantity, item.SalePrice);
                }

                return Json(new ApiResult() { Code = 0, Message = "", Data = OrderID });
            }
            catch (Exception ex)
            {
                return Json(new ApiResult() { Code = 0, Message = ex.Message });
            }
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