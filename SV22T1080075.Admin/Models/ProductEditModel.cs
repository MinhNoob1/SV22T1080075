using SV22T1080075.DomainModels;

namespace SV22T1080075.Admin.Models
{
    /// <summary>
    /// ViewModel dùng cho chức năng bổ sung và cập nhật mặt hàng
    /// </summary>
    public class ProductEditModel : Product
    {
        public IFormFile? UpLoadFile { get; set; }
    }
}
