using Dapper;
using SV22T1080075.DataLayers;
using SV22T1080075.DomainModels;
using System.Data;

namespace SV22T1080075.DataLayers
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu liên quan đến loại hàng (Category)
    /// </summary>
    public class CategoryDAL : BaseDAL
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public CategoryDAL(string connectionString) : base(connectionString)
        {

        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách loại hàng (Category) có phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng mỗi trang (nếu = 0 thì lấy toàn bộ)</param>
        /// <param name="searchValue">Tên loại hàng cần tìm (rỗng nếu lấy tất cả)</param>
        public async Task<IEnumerable<Category>> ListAsync(int page = 1, int pageSize = 0, string searchValue = "")
        {
            if (page < 1) page = 1;
            if (pageSize < 0) pageSize = 0;
            searchValue = $"%{searchValue}%";

            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"WITH cte AS
                            (
	                            SELECT	*,
			                            ROW_NUMBER() OVER(ORDER BY CategoryName) AS RowNumber
	                            FROM	Categories 
	                            WHERE	CategoryName LIKE @searchValue
                            )
                            SELECT * FROM cte
                            WHERE	(@pageSize = 0)
	                            OR	(RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)
                            ORDER BY RowNumber;";

                var parameters = new
                {
                    page,
                    pageSize,
                    searchValue
                };

                return await connection.QueryAsync<Category>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Đếm số lượng loại hàng tìm được
        /// </summary>
        public async Task<int> CountAsync(string searchValue = "")
        {
            searchValue = $"%{searchValue}%";

            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT COUNT(*) 
                            FROM Categories 
                            WHERE CategoryName LIKE @searchValue;";
                return await connection.ExecuteScalarAsync<int>(sql: sql, param: new { searchValue }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một loại hàng
        /// </summary>
        public async Task<Category?> GetAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM Categories WHERE CategoryID = @id;";
                return await connection.QueryFirstOrDefaultAsync<Category>(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Thêm mới một loại hàng
        /// </summary>
        public async Task<int> AddAsync(Category data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"INSERT INTO Categories
                            (
                                CategoryName,
                                Description
                            )
                            VALUES
                            (
                                @CategoryName,
                                @Description
                            );
                            SELECT SCOPE_IDENTITY();";

                return await connection.ExecuteScalarAsync<int>(sql: sql, param: data, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Cập nhật thông tin loại hàng
        /// </summary>
        public async Task<bool> UpdateAsync(Category data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"UPDATE Categories
                            SET     CategoryName = @CategoryName,
                                    Description = @Description
                            WHERE   CategoryID = @CategoryID;";
                return (await connection.ExecuteAsync(sql: sql, param: data, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Xóa một loại hàng
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM Categories WHERE CategoryID = @id;";
                return (await connection.ExecuteAsync(sql: sql, param: new { id }, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Kiểm tra xem loại hàng có đang được sử dụng trong bảng Products hay không
        /// </summary>
        public async Task<bool> InUsed(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"IF EXISTS(SELECT 1 FROM Products WHERE CategoryID = @id)
	                            SELECT 1
                            ELSE
	                            SELECT 0;";
                return await connection.ExecuteScalarAsync<bool>(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
        }
    }
}
