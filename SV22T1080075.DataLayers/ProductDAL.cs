using Dapper;
using SV22T1080075.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.DataLayers
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu liên quan đến sản phẩm (Product)
    /// </summary>
    public class ProductDAL : BaseDAL
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách sản phẩm dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (nếu = 0 thì lấy toàn bộ)</param>
        /// <param name="searchValue">Tên sản phẩm cần tìm (rỗng nếu lấy tất cả)</param>
        public async Task<IEnumerable<Product>> ListAsync(int page = 1, int pageSize = 0, string searchValue = "")
        {
            if (page < 1) page = 1;
            if (pageSize < 0) pageSize = 0;
            searchValue = $"%{searchValue}%";

            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"WITH cte AS
                            (
                                SELECT  *,
                                        ROW_NUMBER() OVER(ORDER BY ProductName) AS RowNumber
                                FROM    Products
                                WHERE   ProductName LIKE @searchValue
                            )
                            SELECT * FROM cte
                            WHERE   (@pageSize = 0)
                                OR  (RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)
                            ORDER BY RowNumber;";

                var parameters = new { page, pageSize, searchValue };
                return await connection.QueryAsync<Product>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Đếm số lượng sản phẩm tìm được
        /// </summary>
        public async Task<int> CountAsync(string searchValue = "")
        {
            searchValue = $"%{searchValue}%";

            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT COUNT(*) 
                            FROM Products
                            WHERE ProductName LIKE @searchValue;";
                return await connection.ExecuteScalarAsync<int>(sql: sql, param: new { searchValue }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một sản phẩm
        /// </summary>
        public async Task<Product?> GetAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM Products WHERE ProductID = @id;";
                return await connection.QueryFirstOrDefaultAsync<Product>(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Thêm một sản phẩm mới
        /// </summary>
        public async Task<int> AddAsync(Product data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"INSERT INTO Products
                            (
                                ProductName,
                                ProductDescription,
                                SupplierID,
                                CategoryID,
                                Unit,
                                Price,
                                Photo,
                                IsSelling
                            )
                            VALUES
                            (
                                @ProductName,
                                @ProductDescription,
                                @SupplierID,
                                @CategoryID,
                                @Unit,
                                @Price,
                                @Photo,
                                @IsSelling
                            );
                            SELECT SCOPE_IDENTITY();";

                return await connection.ExecuteScalarAsync<int>(sql: sql, param: data, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        public async Task<bool> UpdateAsync(Product data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"UPDATE Products
                            SET     ProductName = @ProductName,
                                    ProductDescription = @ProductDescription,
                                    SupplierID = @SupplierID,
                                    CategoryID = @CategoryID,
                                    Unit = @Unit,
                                    Price = @Price,
                                    Photo = @Photo,
                                    IsSelling = @IsSelling
                            WHERE   ProductID = @ProductID;";

                return (await connection.ExecuteAsync(sql: sql, param: data, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Xóa một sản phẩm
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM Products WHERE ProductID = @id;";
                return (await connection.ExecuteAsync(sql: sql, param: new { id }, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Kiểm tra xem sản phẩm có đang được sử dụng trong bảng OrderDetails hay không
        /// </summary>
        public async Task<bool> InUsed(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT CASE WHEN EXISTS(SELECT 1 FROM OrderDetails WHERE ProductID = @id)
                                         THEN 1 ELSE 0 END;";
                return await connection.ExecuteScalarAsync<bool>(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
        }
    }
}
