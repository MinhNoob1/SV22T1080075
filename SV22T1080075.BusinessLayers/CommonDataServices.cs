using SV22T1080075.DataLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.BusinessLayers
{
    /// <summary>
    /// Cung cấp các tính năng giao tiếp, xử lý dữ liệu chung (Province, Supplier, Customer, Shipper, Employee, Category)
    /// </summary>
    public static class CommonDataServices
    {
        private static readonly ProvinceDAL provinceDB;
        private static readonly SupplierDAL supplierDB;
        private static readonly CustomerDAL customerDB;
        private static readonly ShipperDAL shipperDB;
        private static readonly EmployeeDAL employeeDB;
        private static readonly CategoryDAL categoryDB;
        private static readonly ProductDAL productDB;
        /// <summary>
        /// Ctor
        /// (Câu hỏi: )
        /// </summary>
        static CommonDataServices()
        {
            string connectionString = Configuration.ConnectionString;
            provinceDB = new ProvinceDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
            customerDB = new CustomerDAL(connectionString);
            shipperDB = new ShipperDAL(connectionString);
            employeeDB = new EmployeeDAL(connectionString);
            categoryDB = new CategoryDAL(connectionString);
            productDB = new ProductDAL(connectionString);
        }
        /// <summary>
        /// Dữ liệu tỉnh thành
        /// </summary>
        public static ProvinceDAL ProvinceDB => provinceDB;
        /// <summary>
        /// Dữ liệu nhà cung cấp
        /// </summary>
        public static SupplierDAL SupplierDB => supplierDB;
        /// <summary>
        /// Dữ liệu khách hàng
        /// </summary>
        public static CustomerDAL CustomerDB => customerDB;
        /// <summary>
        /// Dữ liệu người giao hàng
        /// </summary>
        public static ShipperDAL ShipperDB => shipperDB;
        /// <summary>
        /// Dữ liệu nhân viên
        /// </summary>
        public static EmployeeDAL EmployeeDB => employeeDB;
        /// <summary>
        /// Dữ liệu loại hàng
        /// </summary>
        public static CategoryDAL CategoryDB => categoryDB;
        public static ProductDAL ProductDB => productDB;
    }
}
