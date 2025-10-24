using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.BusinessLayers
{
    /// <summary>
    /// Dùng để khởi tạo và lưu thông tin cấu hình cho tầng tác nghiệp
    /// </summary>
    public class Configuration
    {
        private static string connectionString = "";
        /// <summary>
        /// Khởi tạo cấu hình cho tầng tác nghiệp
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            Configuration.connectionString = connectionString;
        }
        /// <summary>
        /// Chuỗi tham số kết nối đến CSDL
        /// </summary>
        public static string ConnectionString 
        { 
            get { 
                return connectionString; 
            } 
        }
    }
}
