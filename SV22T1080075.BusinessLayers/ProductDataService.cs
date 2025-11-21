using SV22T1080075.DataLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.BusinessLayers
{
    public static class ProductDataService
    {
        /// <summary>
        /// Cung cấp các tính năng giao tiếp, xử lý dữ liệu chỉ đến mặt hàng
        /// </summary>
        private static readonly ProductDAL productDB;
        /// <summary>
        /// Ctor
        /// </summary>
        static ProductDataService()
        {
            string connectionString = Configuration.ConnectionString;
            productDB = new ProductDAL(connectionString);
        }
        /// <summary>
        /// Dữ liệu mặt hàng
        /// </summary>
        public static ProductDAL ProductDB => productDB;


    }
}
