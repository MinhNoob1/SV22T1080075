using Microsoft.AspNetCore.Mvc;
using SV22T1080075.Admin.Models;
using SV22T1080075.BusinessLayers;
using SV22T1080075.DomainModels;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SV22T1080075.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        const int PAGESIZE = 12;

        public async Task<IActionResult> Index(int page = 1, string searchValue = "")
        {
            var data = await CommonDataServices.EmployeeDB.ListAsync(page, PAGESIZE, searchValue);
            var rowCount = await CommonDataServices.EmployeeDB.CountAsync(searchValue);
            var model = new PaginationSearchResult<Employee>()
            {
                Page = page,
                PageSize = PAGESIZE,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = data
            };
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            var model = new EmployeeEditModel()
            {
                EmployeeID = 0,
                Photo = "nophoto.png"
            };
            return View("Edit", model);
        }

        public async Task<IActionResult> Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhân viên";
            var employee = await CommonDataServices.EmployeeDB.GetAsync(id);
            if (employee == null)
                return RedirectToAction("Index");

            var model = new EmployeeEditModel()
            {
                EmployeeID = employee.EmployeeID,
                EmployeeName = employee.EmployeeName,
                BirthDate = employee.BirthDate,
                Address = employee.Address,
                Email = employee.Email,
                Phone = employee.Phone,
                Photo = employee.Photo,
                IsWorking = employee.IsWorking
            };
            return View(model);
        }

        public async Task<IActionResult> SaveData(EmployeeEditModel model)
        {
            //TODO: Kiểm tra dữ liệu đầu vào
            ViewBag.Title = model.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";
            if (string.IsNullOrWhiteSpace(model.EmployeeName))
                ModelState.AddModelError("EmployeeName", "Tên nhân viên không được để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", "Số điện thoại không được để trống");
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError("Address", "Địa chỉ không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email không được để trống");

            //Thông báo lỗi và yêu cầu nhập lại nếu có trường hợp dữ liệu không hợp lệ
            if (!ModelState.IsValid)
                return View("Edit", model);

            //Nếu có ảnh thì upload ảnh lên và lấy tên file ảnh mới upload cho Photo
            if (model.UpLoadFile != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{model.UpLoadFile.FileName}";
                string filePath = Path.Combine(ApplicationContext.WWWRootPath, @"images\employees", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.UpLoadFile.CopyToAsync(stream);
                }
                model.Photo = fileName;
            }

            Employee data = new Employee()
            {
                EmployeeID = model.EmployeeID,
                EmployeeName = model.EmployeeName,
                BirthDate = model.BirthDate,
                Address = model.Address,
                Email = model.Email,
                Phone = model.Phone,
                Photo = model.Photo,
                IsWorking = model.IsWorking
            };

            if (data.EmployeeID == 0)
            {
                await CommonDataServices.EmployeeDB.AddAsync(data);
            }
            else
            {
                await CommonDataServices.EmployeeDB.UpdateAsync(data);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}