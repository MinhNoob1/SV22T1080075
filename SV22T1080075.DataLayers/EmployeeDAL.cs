using Dapper;
using SV22T1080075.DataLayers;
using SV22T1080075.DomainModels;
using System.Data;

namespace SV22T1080075.DataLayers
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu liên quan đến nhân viên
    /// </summary>
    public class EmployeeDAL : BaseDAL
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public EmployeeDAL(string connectionString) : base(connectionString)
        {

        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhân viên dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (nếu = 0 thì lấy toàn bộ)</param>
        /// <param name="searchValue">Tên nhân viên cần tìm (rỗng nếu lấy toàn bộ)</param>
        public async Task<IEnumerable<Employee>> ListAsync(int page = 1, int pageSize = 0, string searchValue = "")
        {
            if (page < 1) page = 1;
            if (pageSize < 0) pageSize = 0;
            searchValue = $"%{searchValue}%";

            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"WITH cte AS
                            (
	                            SELECT	*,
			                            ROW_NUMBER() OVER(ORDER BY FullName) AS RowNumber
	                            FROM	Employees 
	                            WHERE	FullName LIKE @searchValue
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
                return await connection.QueryAsync<Employee>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Đếm số lượng nhân viên phù hợp với điều kiện tìm kiếm
        /// </summary>
        public async Task<int> CountAsync(string searchValue = "")
        {
            searchValue = $"%{searchValue}%";
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT COUNT(*) 
                            FROM Employees 
                            WHERE FullName LIKE @searchValue;";
                return await connection.ExecuteScalarAsync<int>(sql: sql, param: new { searchValue }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của 1 nhân viên
        /// </summary>
        public async Task<Employee?> GetAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"SELECT * FROM Employees WHERE EmployeeID = @id;";
                return await connection.QueryFirstOrDefaultAsync<Employee>(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Thêm một nhân viên mới (không bao gồm Password và RoleNames)
        /// </summary>
        public async Task<int> AddAsync(Employee data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"INSERT INTO Employees
                            (
                                FullName,
                                BirthDate,
                                Address,
                                Phone,
                                Email,
                                Photo,
                                IsWorking
                            )
                            VALUES
                            (
                                @FullName,
                                @BirthDate,
                                @Address,
                                @Phone,
                                @Email,
                                @Photo,
                                @IsWorking
                            );
                            SELECT SCOPE_IDENTITY();";
                return await connection.ExecuteScalarAsync<int>(sql: sql, param: data, commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên (không bao gồm Password và RoleNames)
        /// </summary>
        public async Task<bool> UpdateAsync(Employee data)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"UPDATE Employees
                            SET     FullName = @FullName,
                                    BirthDate = @BirthDate,
                                    Address = @Address,
                                    Phone = @Phone,
                                    Email = @Email,
                                    Photo = @Photo,
                                    IsWorking = @IsWorking
                            WHERE   EmployeeID = @EmployeeID;";
                return (await connection.ExecuteAsync(sql: sql, param: data, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Xóa một nhân viên
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"DELETE FROM Employees WHERE EmployeeID = @id;";
                return (await connection.ExecuteAsync(sql: sql, param: new { id }, commandType: CommandType.Text)) > 0;
            }
        }

        /// <summary>
        /// Kiểm tra xem nhân viên có đang được sử dụng trong bảng Orders hay không
        /// </summary>
        public async Task<bool> InUsed(int id)
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = @"IF EXISTS(SELECT 1 FROM Orders WHERE EmployeeID = @id)
	                            SELECT 1
                            ELSE
	                            SELECT 0;";
                return await connection.ExecuteScalarAsync<bool>(sql: sql, param: new { id }, commandType: CommandType.Text);
            }
        }
    }
}
