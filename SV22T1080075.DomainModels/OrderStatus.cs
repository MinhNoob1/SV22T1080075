using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.DomainModels
{
    /// <summary>
    /// Lớp định nghĩa các trạng thái của đơn đặt hàng
    /// </summary>
    public static class OrderStatus
    {
        /// <summary>
        /// Đơn hàng vừa được khởi tạo (đang chờ duyệt)
        /// </summary>
        public const int ORDER_INIT = 1;
        /// <summary>
        /// Đơn hàng đã được duyệt (đang chờ chuyển hàng)
        /// </summary>
        public const int ORDER_ACCEPTED = 2;
        /// <summary>
        /// Đơn hàng đang được chuyển hàng
        /// </summary>
        public const int ORDER_SHIPPING = 3;
        /// <summary>
        /// Đơn hàng đã hoàn tất
        /// </summary>
        public const int ORDER_FINISHED = 4;
        /// <summary>
        /// Đơn hàng bị hủy
        /// </summary>
        public const int ORDER_CANCEL = -1;
        /// <summary>
        /// Đơn hàng bị từ chối
        /// </summary>
        public const int ORDER_REJECTED = -2;
    }
}
