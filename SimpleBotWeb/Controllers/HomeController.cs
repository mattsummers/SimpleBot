using Microsoft.AspNetCore.Mvc;
using SimpleBotWeb.Models;
using System.Diagnostics;
using SimpleBotWeb.Models.Views.Home;

namespace SimpleBotWeb.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var model = new HomeIndexViewModel();
            model.Load();
            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
