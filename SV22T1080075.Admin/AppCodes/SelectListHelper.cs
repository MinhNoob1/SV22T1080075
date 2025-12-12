using Microsoft.AspNetCore.Mvc.Rendering;
using SV22T1080075.BusinessLayers;

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
    }
}
