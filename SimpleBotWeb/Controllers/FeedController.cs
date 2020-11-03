using Microsoft.AspNetCore.Mvc;
using SimpleBotWeb.Models.Views.List;

namespace SimpleBotWeb.Controllers
{
    public class FeedController : BaseController
    {
        public IActionResult List()
        {
            var model = new FeedListViewModel();
            model.Load();
            return Json(model.Replies);
        }

        public IActionResult Insults()
        {
            var model = new FeedInsultsViewModel();
            model.Load();
            return Json(model.RandomInsults);
        }
    }
}