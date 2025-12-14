using Microsoft.AspNetCore.Mvc.Rendering;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;

namespace SV22T1080075.Admin
{
    public class SelectListHelper
    {
        /// <summary>
        /// Danh sách các tỉnh thành cho thẻ select 
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<SelectListItem>> Provinces()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "", Text = "-- Chọn Tỉnh/Thành --" });
            foreach (var item in await CommonDataServices.ProvinceDB.ListAsync())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName.ToString(),
                    Text = item.ProvinceName,
                });
            }
            return list;
        }
        /// <summary>
        /// Danh sách các khách hàng
        /// </summary>
        /// <returns></returns>
        public async static Task<IEnumerable<SelectListItem>> Customers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "", Text = "-- Khách hàng --" });
            foreach (var item in await CommonDataServices.CustomerDB.ListAsync())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CustomerName.ToString(),
                    Text = item.CustomerName,
                });
            }
            return list;
        }
        /// <summary>
        /// Danh sách các loại hàng
        /// </summary>
        /// <returns></returns>
        public async static Task<IEnumerable<SelectListItem>> Category()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "", Text = "-- Loại hàng --" });
            foreach (var item in await CommonDataServices.CategoryDB.ListAsync())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CategoryName.ToString(),
                    Text = item.CategoryName,
                });
            }
            return list;
        }
        /// <summary>
        /// Danh sách các nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public async static Task<IEnumerable<SelectListItem>> Supplier()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "", Text = "-- Nhà cung cấp --" });
            foreach (var item in await CommonDataServices.SupplierDB.ListAsync())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierName.ToString(),
                    Text = item.SupplierName,
                });
            }
            return list;
        }
        /// <summary>
        /// Danh sách các trạng thái đơn đặt hàng
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ListOfOrderStatus()
        {
            var list = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Trạng thái --" },
            new SelectListItem { Value = OrderStatus.ORDER_INIT.ToString(), Text = new Order { Status = OrderStatus.ORDER_INIT }.StatusDescription },
            new SelectListItem { Value = OrderStatus.ORDER_ACCEPTED.ToString(), Text = new Order { Status = OrderStatus.ORDER_ACCEPTED }.StatusDescription },
            new SelectListItem { Value = OrderStatus.ORDER_SHIPPING.ToString(), Text = new Order { Status = OrderStatus.ORDER_SHIPPING }.StatusDescription },
            new SelectListItem { Value = OrderStatus.ORDER_FINISHED.ToString(), Text = new Order { Status = OrderStatus.ORDER_FINISHED }.StatusDescription },
            new SelectListItem { Value = OrderStatus.ORDER_CANCEL.ToString(), Text = new Order { Status = OrderStatus.ORDER_CANCEL }.StatusDescription },
            new SelectListItem { Value = OrderStatus.ORDER_REJECTED.ToString(), Text = new Order { Status = OrderStatus.ORDER_REJECTED }.StatusDescription }
        };
            return list;
        }
    }
}
