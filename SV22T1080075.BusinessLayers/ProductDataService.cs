using SV22T1080075.DataLayers;
using SV22T1080075.DomainModels;
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
        private static readonly CategoryDAL categoryDB;
        private static readonly SupplierDAL supplierDB;
        private static readonly ProductPhotoDAL photoDB;
        private static readonly ProductAttributeDAL attributeDB;
        /// <summary>
        /// Ctor
        /// </summary>
        static ProductDataService()
        {
            string connectionString = Configuration.ConnectionString;
            productDB = new ProductDAL(connectionString);
            categoryDB = new CategoryDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
            photoDB = new ProductPhotoDAL(connectionString);
            attributeDB = new ProductAttributeDAL(connectionString);
        }
        /// <summary>
        /// Dữ liệu mặt hàng
        /// </summary>
        public static ProductDAL ProductDB => productDB;
        public static int Add(Product data) => productDB.AddAsync(data).Result;
        public static bool Update(Product data) => productDB.UpdateAsync(data).Result;
        public static IList<Category> ListCategories() => categoryDB.ListAsync().Result.ToList();
        public static IList<Supplier> ListSuppliers() => supplierDB.ListAsync().Result.ToList();
        public static IList<ProductPhoto> ListPhotos(int productId) => photoDB.ListAsync(productId).Result.ToList();
        public static IList<ProductAttribute> ListAttributes(int productId) => attributeDB.ListAsync(productId).Result.ToList();

    }
}
