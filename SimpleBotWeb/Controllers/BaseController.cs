using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleBotWeb.Models.Helpers;
using SimpleBotWeb.Models.Values;
using System.Linq;
using System.Security.Claims;

namespace SimpleBotWeb.Controllers
{
    public class BaseController : Controller
    {
        public bool IsAuthenticated { get; set; }
        public int MemberId { get; set; }
        public string Email { get; set; }
        public bool IsAdministrator { get; set; }

        public BaseController()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            ViewBag.IsAuthenticated = IsAuthenticated = (ctx.HttpContext.User.Identity.IsAuthenticated);
            ViewBag.Username = Email = ctx.HttpContext.User.Identity.Name ?? "";
            ViewBag.MemberId = MemberId = ValidationHelper.ParseInt(ctx.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            ViewBag.IsAdministrator = IsAdministrator = ctx.HttpContext.User.IsInRole(UserRole.Administrator.ToString());
            ViewBag.Configuration = AppConfiguration.SiteConfiguration;
        }
    }
}