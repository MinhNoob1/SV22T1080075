using Microsoft.Data.SqlClient;

namespace SV22T1080075.DataLayers
{
    /// <summary>
    /// Lớp cơ sở cho các lớp xử lý dữ liệu trên CSDL SQL Server
    /// </summary>
    public abstract class BaseDAL
    {
        protected string connectionString;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString">Chuỗi tham số kết nối đến CSDL</param>
        public BaseDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// Mở kết nối đến CSDL
        /// </summary>
        /// <returns>đồng bộ</returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
        }
        /// <summary>
        /// Mở kết nối đến CSDL 
        /// </summary>
        /// <returns>bất đồng bộ</returns>
        protected async Task<SqlConnection> OpenConnectionAsync()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            await connection.OpenAsync();
            return connection;
        }
    }
}
