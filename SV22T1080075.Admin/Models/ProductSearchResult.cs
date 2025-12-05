using SV22T1080075.DomainModels;

namespace SV22T1080075.Admin.Models
{
    /// <summary>
    /// ViewModel chuyên dùng cho tìm kiếm mặt hàng có phân trang
    /// </summary>
    public class ProductSearchResult
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Số dòng trên 1 trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số dòng tìm được
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 1;
                if (RowCount == 0) return 1;

                int pages = RowCount / PageSize;
                if (RowCount % PageSize > 0) pages++;

                return pages;
            }
        }

        /// <summary>
        /// Từ khóa tìm kiếm theo tên
        /// </summary>
        public string SearchValue { get; set; } = "";

        /// <summary>
        /// Lọc theo loại hàng
        /// </summary>
        public int CategoryID { get; set; } = 0;

        /// <summary>
        /// Lọc theo nhà cung cấp
        /// </summary>
        public int SupplierID { get; set; } = 0;

        /// <summary>
        /// Lọc theo giá tối thiểu
        /// </summary>
        public decimal MinPrice { get; set; } = 0;

        /// <summary>
        /// Lọc theo giá tối đa
        /// </summary>
        public decimal MaxPrice { get; set; } = 0;

        /// <summary>
        /// Dữ liệu sản phẩm trả về
        /// </summary>
        public required IEnumerable<Product> Data { get; set; }
    }
}
