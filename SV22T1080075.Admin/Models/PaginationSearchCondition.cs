namespace SV22T1080075.Admin.Models
{
    /// <summary>
    /// Đầu vào sử dụng cho tìm kiếm và phân trang dữ liệu
    /// </summary>
    public class PaginationSearchCondition
    {
        /// <summary>
        /// Trang cần hiển thị
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Số dòng trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Giá trị cần tìm
        /// </summary>
        public string SearchValue { get; set; } = "";
    }
    /// <summary>
    /// Đầu vào tìm kiếm, phân trang dối với mặt hàng
    /// </summary>
    public class ProductSearchCondition : PaginationSearchCondition
    {
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public decimal minPrice { get; set; }
        public decimal maxPrice { get; set; }
    }
}
