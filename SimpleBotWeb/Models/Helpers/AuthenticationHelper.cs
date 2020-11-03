using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SimpleBotWeb.Models.DataObjects;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleBotWeb.Models.Helpers
{
    public class AuthenticationHelper
    {
        public static async Task Login(HttpContext request, User userAccount)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAccount.Name, ClaimValueTypes.String),
                new Claim(ClaimTypes.Email, userAccount.Email, ClaimValueTypes.String),
                new Claim(ClaimTypes.NameIdentifier, userAccount.MemberId.ToString(), ClaimValueTypes.Integer32)
            };

            foreach (var role in userAccount.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.ToString()));
            }

            var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var cp = new ClaimsPrincipal(ci);
            await request.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(cp));
        }
    }
}