using Dapper;
using SV22T1080075.DomainModels;

namespace SV22T1080075.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lý dữ liệu liên quan đến khách hàng
    /// </summary>
    public class CustomerDAL : BaseDAL
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {

        }
        public async Task<IEnumerable<Customer>> ListAsync()
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = "SELECT * FROM Customers";
                return await connection.QueryAsync<Customer>(sql);
            }
        }
    }
}
