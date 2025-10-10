using Dapper;
using SV22T1080075.DomainModels;

namespace SV22T1080075.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lý dữ liệu liên quan đến tỉnh thành
    /// </summary>
    public class ProvinceDAL : BaseDAL
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public ProvinceDAL(string connectionString) : base(connectionString)
        {

        }
        /// <summary>
        /// Lấy danh sách tỉnh thành
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Province>> ListAsync()
        {
            using (var connection = await OpenConnectionAsync())
            {
                var sql = "SELECT * FROM Provinces";
                return await connection.QueryAsync<Province>(sql);
            }
        }
    }
}
