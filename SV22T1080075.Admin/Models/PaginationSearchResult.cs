namespace SV22T1080075.Admin.Models
{
    /// <summary>
    /// ViewModel phân trang dùng chung
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationSearchResult<T> where T : class
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Số dòng trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string SearchValue { get; set; } = "";
        /// <summary>
        /// Tổng số dòng tìm được
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize <= 0 || RowCount <= 0) return 1;
                int pages = RowCount / PageSize;
                if (RowCount % PageSize > 0) pages++;
                return pages;
            }
        }
        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public required IEnumerable<T> Data { get; set; }
    }
}
