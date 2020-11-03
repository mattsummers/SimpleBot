using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Helpers;
using SimpleBotWeb.Models.Views.Admin;

namespace SimpleBotWeb.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Config()
        {
            var model = new AdminConfigViewModel();
            model.Load();
            return View(model);
        }

        public IActionResult SaveConfig(string footerText, string homepageText)
        {
            var model = new AdminConfigViewModel();
            var config = new Configuration();
            config["FooterText"] = footerText;
            config["HomepageText"] = homepageText;
            model.Save(config);
            TempData["message"] = "Saved";
            return RedirectToAction("Index");
        }

        public IActionResult News()
        {
            var model = new AdminNewsViewModel();
            model.Load();
            return View(model);
        }

        [HttpGet]
        public IActionResult NewsCreate()
        {
            var model = new AdminNewsViewModel();
            return View("NewsEdit", model);
        }

        [HttpGet]
        public IActionResult NewsEdit(int id)
        {
            var model = new AdminNewsViewModel();
            model.LoadNewsEntry(id);
            TempData["message"] = model.Message;
            TempData["error"] = model.Error;
            if (!model.Success)
            {
                return RedirectToAction("News");
            }
            return View(model);
        }

        public IActionResult NewsEditSave(int id, string datePostedUtc, string content)
        {
            var model = new AdminNewsViewModel();
            var news = new NewsEntry();
            news.NewsId = id;
            news.DatePostedUtc = ValidationHelper.ParseDateTime(datePostedUtc);
            news.Content = content ?? "";
            model.Save(news);
            
            TempData["message"] = model.Message;
            TempData["error"] = model.Error;

            return RedirectToAction("News", model);
        }

        [HttpPost]
        public IActionResult NewsDelete(int id)
        {
            var model = new AdminNewsViewModel();
            model.Delete(id);

            TempData["message"] = model.Message;
            TempData["error"] = model.Error;

            return RedirectToAction("News");
        }
    }
}