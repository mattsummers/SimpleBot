using Microsoft.AspNetCore.Mvc;
using SimpleBotWeb.Models.Helpers;
using SimpleBotWeb.Models.Views.Account;
using System.Threading.Tasks;

namespace SimpleBotWeb.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public IActionResult Login()
        {
            var model = new AccountLoginViewModel();
            return View("Login", model);
        }

        [HttpPost]
        [ActionName("Login")]
        public IActionResult Login_Post(string email, string password)
        {
            var model = new AccountLoginViewModel();
            model.AttemptLogon(email, password, HttpContext);
            TempData["message"] += model.Message;
            TempData["error"] += model.Error;

            if (model.Success)
            {
                return RedirectToAction("Replies", "Entries");
            }
            return View("Login", model);
        }


        [HttpGet]
        public IActionResult Logout()
        {
            var model = new AccountLogoutViewModel();
            return View("Logout", model);
        }

        [HttpPost]
        [ActionName("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout_Post()
        {
            var model = new AccountLogoutViewModel();
            await model.Logoff(HttpContext);
            TempData["message"] += model.Message;
            TempData["error"] += model.Error;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult GetPasswordStrength(string password = "")
        {
            password = (password ?? "").Trim();
            var passwordScore = PasswordHelper.GetPasswordStrength(password);
            var result = new
            {
                Score = passwordScore,
                Valid = passwordScore >= PasswordHelper.CutoffStrength,
                Message = PasswordHelper.TranslatePasswordScore(passwordScore)
            };

            return Json(result);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var model = new AccountForgotPasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ActionName("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPasswordPost(string email = "")
        {
            var model = new AccountForgotPasswordViewModel();
            email = email.Trim();
            model.SendEmail(Url, email);

            TempData["message"] += model.Message;
            TempData["error"] += model.Error;

            if (model.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("ForgotPassword", model);
        }

        [HttpGet]
        public IActionResult ResetPassword(int memberId, string key)
        {
            return View(new AccountResetPasswordViewModel(memberId, key));
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPasswordPost(int memberId, string key, string password = "")
        {
            password = password.Trim();
            var model = new AccountResetPasswordViewModel(memberId, key);
            model.ResetPassword(password);

            if (model.Success)
            {
                await AuthenticationHelper.Login(HttpContext, model.UserInfo);
                return RedirectToAction("Replies", "Entries");
            }

            TempData["message"] += model.Message;
            TempData["error"] += model.Error;

            return View("ResetPassword", model);
        }
    }
}