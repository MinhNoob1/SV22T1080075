using SV22T1080075.DataLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.BusinessLayers
{
    /// <summary>
    /// Các chức năng tác nghiệp liên quan đến đơn hàng
    /// </summary>
    public class OrderDataService
    {
        private static readonly OrderDAL orderDB;
        /// <summary>
        /// 
        /// </summary>
        static OrderDataService()
        {
            orderDB = new OrderDAL(Configuration.ConnectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        public static OrderDAL OrderDB => orderDB;
    }
}
