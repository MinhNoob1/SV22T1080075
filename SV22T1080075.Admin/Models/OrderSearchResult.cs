using SV22T1080075.DomainModels;

namespace SV22T1080075.Admin.Models
{
    /// <summary>
    /// Kết quả tìm kiếm đơn hàng có phân trang
    /// </summary>
    public class OrderSearchResult : PaginationSearchResult<Order>
    {
        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Từ ngày
        /// </summary>
        public DateTime FromDate { get; set; }
        /// <summary>
        /// Đến ngày
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
