using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.DomainModels
{
    /// <summary>
    /// Mặt hàng
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Mã mặt hàng
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Tên mặt hàng
        /// </summary>
        public string ProductName { get; set; } = "";
        /// <summary>
        /// Mô tả mặt hàng
        /// </summary>
        public string ProductDescription { get; set; } = "";
        /// <summary>
        /// Mã nhà cung cấp
        /// </summary>
        public int SupplierId { get; set; }
        /// <summary>
        /// Mã loại hàng
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Đơn vị
        /// </summary>
        public string Unit { get; set; } = "";
        /// <summary>
        /// Giá thành mặt hàng
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Hình ảnh mặt hàng
        /// </summary>
        public string Photo { get; set; } = "";
        /// <summary>
        /// Sản phẩm có đang bán không?
        /// </summary>
        public bool IsSelling { get; set; }
    }
}
