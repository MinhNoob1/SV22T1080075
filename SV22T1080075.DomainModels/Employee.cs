using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1080075.DomainModels
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string EmployeeName { get; set; } = "";
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// Địa chỉ nhà
        /// </summary>
        public string Address { get; set; } = "";
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; } = "";
        /// <summary>
        /// Địa chỉ Email
        /// </summary>
        public string Email { get; set; } = "";
        /// <summary>
        /// File ảnh đại diện
        /// </summary>
        public string Photo { get; set; } = "";
        /// <summary>
        /// Có đang làm việc không
        /// </summary>
        public int IsWorking { get; set; }
    }
}
