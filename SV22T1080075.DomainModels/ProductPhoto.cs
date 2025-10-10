using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.DomainModels
{
    /// <summary>
    /// Ảnh mặt hàng
    /// </summary>
    public class ProductPhoto
    {
        /// <summary>
        /// Mã hình ảnh
        /// </summary>
        public int PhotoId { get; set; }
        /// <summary>
        ///  Mã mặt hàng
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// File hình ảnh
        /// </summary>
        public string Photo { get; set; } = "";
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; } = "";
        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Có hiện hình ảnh sản phẩm?
        /// </summary>
        public bool isHidden { get; set; }

    }
}
