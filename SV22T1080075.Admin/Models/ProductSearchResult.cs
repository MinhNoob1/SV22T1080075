using SV22T1080075.DomainModels;

namespace SV22T1080075.Admin.Models
{
    /// <summary>
    /// Kết quả tìm kiếm sản phẩm có phân trang
    /// </summary>
    public class ProductSearchResult : PaginationSearchResult<Product>
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
        public decimal MinPrice { get; set; }
        /// <summary>
        /// Giá trị tối đa
        /// </summary>
        public decimal MaxPrice { get; set; }
    }
}
