using Microsoft.AspNetCore.Mvc.Rendering;
using SV22T1080075.DomainModels;

namespace SV22T1080075.Admin.Models
{
    /// <summary>
    /// Đầu vào sử dụng cho tìm kiếm và phân trang dữ liệu
    /// </summary>
    public class PaginationSearchCondition
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Số dòng trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Tìm kiếm theo tên
        /// </summary>
        public string SearchValue { get; set; } = "";
    }
    /// <summary>
    /// Đầu vào tìm kiếm, phân trang dối với mặt hàng
    /// </summary>
    public class ProductSearchCondition : PaginationSearchCondition
    {
        /// <summary>
        /// Mã loại hàng
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Mã nhà cung cấp
        /// </summary>
        public int SupplierID { get; set; }
        /// <summary>
        /// Giá trị tối thiểu
        /// </summary>
        public decimal minPrice { get; set; }
        /// <summary>
        /// Giá trị tối đa
        /// </summary>
        public decimal maxPrice { get; set; }
    }
    public class OrderSearchCondition : PaginationSearchCondition
    {
        /// <summary>
        /// Trạng thái đơn hàng cần tìm
        /// </summary>
        public int Status { get; set; } = 0;
        /// <summary>
        /// Khoảng thời gian cần tìm từ bắt đầu đến đi của đơn đặt hàng
        /// </summary>
        public string DateRange { get; set; } = "";
        /// <summary>
        /// Từ ngày
        /// </summary>
        public DateTime? FormDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DateRange)) return null;
                var parts = DateRange.Split('-', StringSplitOptions.TrimEntries);
                if (parts.Length != 2) return null;
                if (DateTime.TryParse(parts[0], out DateTime from))
                    return from;
                return null;
            }
        }
        /// <summary>
        /// Đến ngày
        /// </summary>
        public DateTime? ToDate 
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DateRange)) return null;
                var parts = DateRange.Split('-', StringSplitOptions.TrimEntries);
                if (parts.Length != 2) return null;
                if (DateTime.TryParse(parts[1], out DateTime to))
                    return to;
                return null;
                /*
                try
                {
                    string[] values = DateRange.Split("-");
                    DateTime day = DateTime.Parse(values[1].Trim());
                    return day;
                }
                catch
                {
                    return DateTime.Today;
                }
                */
            } 
        }
    }
}
