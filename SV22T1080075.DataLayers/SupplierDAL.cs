using Dapper;
using SV22T1080075.DataLayers;
using SV22T1080075.DomainModels;
using System.Data;

namespace SV22T1080075.DataLayers
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu liên quan đến nhà cung cấp
    /// </summary>
    public class SupplierDAL : BaseDAL
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public SupplierDAL(string connectionString) : base(connectionString)
        {

        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhà cung cấp dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (nếu = 0 thì lấy toàn bộ)</param>
        /// <param name="searchValue">Tên nhà cung cấp hoặc người liên hệ cần tìm (rỗng nếu lấy tất cả)</param>
        public async Task<IEnumerable<Supplier>> ListAsync(int page = 1, int pageSize = 0, string searchValue = "")
        {
            if (page < 1) page = 1;
            if (pageSize < 0) pageSize = 0;
            searchValue = $"%{searchValue}%";

            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"WITH cte AS
                            (
                                SELECT  *,
                                        ROW_NUMBER() OVER(ORDER BY SupplierName) AS RowNumber
                                FROM    Suppliers
                                WHERE   SupplierName LIKE @searchValue 
                                    OR  ContactName LIKE @searchValue
                            )
                            SELECT * FROM cte
                            WHERE   (@pageSize = 0)
                                OR  (RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)
                            ORDER BY RowNumber;";
                var parameters = new
                {
                    page,
                    pageSize,
                    searchValue
                };
                return await connection.QueryAsync<Supplier>(sql, parameters, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Đếm số lượng nhà cung cấp tìm được
        /// </summary>
        public async Task<int> CountAsync(string searchValue = "")
        {
            searchValue = $"%{searchValue}%";
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT COUNT(*) 
                            FROM Suppliers
                            WHERE SupplierName LIKE @searchValue 
                               OR ContactName LIKE @searchValue;";
                var parameters = new { searchValue };
                return await connection.ExecuteScalarAsync<int>(sql, parameters, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một nhà cung cấp
        /// </summary>
        public async Task<Supplier?> GetAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM Suppliers WHERE SupplierID = @id";
                var parameters = new { id };
                return await connection.QueryFirstOrDefaultAsync<Supplier>(sql, parameters, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Thêm một nhà cung cấp mới và trả về ID vừa thêm
        /// </summary>
        public async Task<int> AddAsync(Supplier data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"INSERT INTO Suppliers
                            (
                                SupplierName,
                                ContactName,
                                Province,
                                Address,
                                Phone,
                                Email
                            )
                            VALUES
                            (
                                @SupplierName,
                                @ContactName,
                                @Province,
                                @Address,
                                @Phone,
                                @Email
                            );
                            SELECT SCOPE_IDENTITY();";
                var parameters = new { data };
                return await connection.ExecuteScalarAsync<int>(sql, parameters, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Cập nhật thông tin của một nhà cung cấp
        /// </summary>
        public async Task<bool> UpdateAsync(Supplier data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"UPDATE Suppliers
                            SET     SupplierName = @SupplierName,
                                    ContactName = @ContactName,
                                    Province = @Province,
                                    Address = @Address,
                                    Phone = @Phone,
                                    Email = @Email
                            WHERE   SupplierID = @SupplierID;";
                var parameters = new { data };
                return (await connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Xóa một nhà cung cấp
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM Suppliers WHERE SupplierID = @id;";
                var parameters = new { id };
                return (await connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Kiểm tra xem nhà cung cấp có đang được sử dụng trong bảng sản phẩm không
        /// </summary>
        public async Task<bool> InUsed(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"IF EXISTS(SELECT 1 FROM Products WHERE SupplierID = @id)
	                            SELECT 1
                            ELSE
	                            SELECT 0;";
                var parameters = new { id };
                return await connection.ExecuteScalarAsync<bool>(sql, parameters, commandType: CommandType.Text);
            }
        }
    }
}
