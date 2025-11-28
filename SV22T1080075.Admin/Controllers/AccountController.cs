using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SV22T1080075.BusinessLayers;
using Microsoft.AspNetCore.Authorization;

namespace SV22T1080075.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous, HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.UserName = username;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập email và mật khẩu");
                return View();
            }

            //Kiểm tra thông tin đăng nhập
            var userAccount = await UserAccountService.EmployeeUserAccountDB.AuthenticateAsync(username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }

            //Tạo thông tin để ghi trong "giấy chứng nhận"
            WebUserData userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,
                Roles = userAccount.RoleNames.Split(',').ToList()
            };

            //Thiết lập phiên đăng nhập 
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());

            //Quay về trang chủ
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

