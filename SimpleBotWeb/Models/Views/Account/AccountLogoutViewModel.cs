using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SimpleBotWeb.Models.Views.Account
{
    public class AccountLogoutViewModel
    {
        public AccountLogoutViewModel()
        {
            Success = true;
            Message = Error = "";
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public async Task<bool> Logoff(HttpContext httpContext)
        {
            Message = "If you bother me again, I'll hack your bank account";
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return true;
        }
    }
}