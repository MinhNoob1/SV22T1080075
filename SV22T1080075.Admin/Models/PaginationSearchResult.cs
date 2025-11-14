namespace SV22T1080075.Admin.Models
{
    /// <summary>
    /// Biểu diễn cho dữ liệu đầu ra khi tìm kiếm dưới dạng phân trang (ViewModel)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationSearchResult<T> where T : class
    {
        /// <summary>
        /// Trang được hiển thị
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Số dòng trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Giá trị tìm kiếm
        /// </summary>
        public string SearchValue { get; set; } = "";
        /// <summary>
        /// Số dòng dữ liệu tìm được
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
                if (RowCount <= 0) return 1;
                int page = RowCount / PageSize;
                if (RowCount % PageSize > 0) page += 1;
                return page;
            }
        }
        /// <summary>
        /// Danh sách dữ liệu truy vấn được
        /// </summary>
        public required IEnumerable<T> Data { get; set;}
    }
}
