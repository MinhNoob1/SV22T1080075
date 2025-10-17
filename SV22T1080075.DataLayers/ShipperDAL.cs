using Dapper;
using SV22T1080075.DataLayers;
using SV22T1080075.DomainModels;
using System.Data;

namespace SV22T1080075.DataLayers
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu liên quan đến đơn vị vận chuyển (Shipper)
    /// </summary>
    public class ShipperDAL : BaseDAL
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public ShipperDAL(string connectionString) : base(connectionString)
        {

        }
        /// <summary>
        /// Lấy danh sách Shipper có phân trang và tìm kiếm
        /// </summary>
        /// <param name="page">Trang cần lấy</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (nếu = 0 thì lấy toàn bộ)</param>
        /// <param name="searchValue">Tên Shipper cần tìm (rỗng nếu lấy tất cả)</param>
        public async Task<IEnumerable<Shipper>> ListAsync(int page = 1, int pageSize = 0, string searchValue = "")
        {
            if (page < 1) page = 1;
            if (pageSize < 0) pageSize = 0;
            searchValue = $"%{searchValue}%";

            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"WITH cte AS
                            (
	                            SELECT	*,
			                            ROW_NUMBER() OVER(ORDER BY ShipperName) AS RowNumber
	                            FROM	Shippers 
	                            WHERE	ShipperName LIKE @searchValue
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
                return await connection.QueryAsync<Shipper>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Đếm số lượng Shipper tìm được
        /// </summary>
        public async Task<int> CountAsync(string searchValue = "")
        {
            searchValue = $"%{searchValue}%";
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT COUNT(*) 
                            FROM Shippers 
                            WHERE ShipperName LIKE @searchValue;";
                return await connection.ExecuteScalarAsync<int>(sql: sql, param: new { searchValue }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của 1 Shipper
        /// </summary>
        public async Task<Shipper?> GetAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM Shippers WHERE ShipperID = @id;";
                return await connection.QueryFirstOrDefaultAsync<Shipper>(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Thêm 1 Shipper mới, trả về ID vừa thêm
        /// </summary>
        public async Task<int> AddAsync(Shipper data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"INSERT INTO Shippers
                            (
                                ShipperName,
                                Phone
                            )
                            VALUES
                            (
                                @ShipperName,
                                @Phone
                            );
                            SELECT SCOPE_IDENTITY();";
                return await connection.ExecuteScalarAsync<int>(sql: sql, param: data, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Cập nhật thông tin Shipper
        /// </summary>
        public async Task<bool> UpdateAsync(Shipper data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"UPDATE Shippers
                            SET     ShipperName = @ShipperName,
                                    Phone = @Phone
                            WHERE   ShipperID = @ShipperID;";
                return (await connection.ExecuteAsync(sql: sql, param: data, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Xóa 1 Shipper
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM Shippers WHERE ShipperID = @id;";
                return (await connection.ExecuteAsync(sql: sql, param: new { id }, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Kiểm tra Shipper có đang được sử dụng trong bảng Orders hay không
        /// </summary>
        public async Task<bool> InUsed(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"IF EXISTS(SELECT 1 FROM Orders WHERE ShipperID = @id)
	                            SELECT 1
                            ELSE
	                            SELECT 0;";
                return await connection.ExecuteScalarAsync<bool>(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
        }
    }
}
