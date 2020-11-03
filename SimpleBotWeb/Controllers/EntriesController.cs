using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBotWeb.Models.Views.Entries;

namespace SimpleBotWeb.Controllers
{
    [Authorize]
    public class EntriesController : BaseController
    {
        public IActionResult Replies()
        {
            var model = new EntriesRepliesViewModel(MemberId, IsAdministrator);
            model.Load();
            return View("Replies", model);
        }

        [HttpPost]
        public IActionResult Add(int entryId = 0, bool startsWith = false, string phrase = "", bool hidden = false, string response = "", bool allowRepeat = false)
        {
            if (entryId > 0)
            {
                var editModel = new EntriesEditViewModel();
                editModel.Edit(entryId, MemberId, phrase, response, startsWith, hidden, allowRepeat);
                TempData["message"] += editModel.Message;
                TempData["error"] += editModel.Error;
                return Redirect(Url.Action("Replies") + "#entry" + phrase);
            }

            var model = new EntriesAddViewModel();
            model.Add(phrase, response, startsWith, hidden, MemberId, allowRepeat);
            TempData["message"] += model.Message;
            TempData["error"] += model.Error;
            return Redirect(Url.Action("Replies") + "#entry" + phrase);
        }

        [HttpGet]
        public IActionResult LoadReply(int entryId)
        {
            var model = new EntriesLoadReplyViewModel();
            model.Load(entryId);
            return Json(model.Entry);
        }

        [HttpPost]
        public IActionResult Delete(int entryId)
        {
            var model = new EntriesEditViewModel();
            model.Delete(entryId);
            TempData["message"] += model.Message;
            TempData["error"] += model.Error;
            return Redirect(Url.Action("Replies") + "#entry" + model.Phrase);
        }

        [HttpGet]
        public IActionResult RandomInsults()
        {
            var model = new EntriesRandomInsultsViewModel();
            model.Load();
            TempData["message"] += model.Message;
            TempData["error"] += model.Error;
            return View("RandomInsults", model);
        }

        [HttpPost]
        [ActionName("RandomInsults")]
        public IActionResult RandomInsults_Post(string insult)
        {
            var model = new EntriesRandomInsultsViewModel();
            model.Add(insult);
            model.Load();
            TempData["message"] += model.Message;
            TempData["error"] += model.Error;
            return RedirectToAction("RandomInsults");
        }

        [HttpPost]
        public IActionResult RandomInsultsDelete(int randomInsultId)
        {
            var model = new EntriesRandomInsultsViewModel();
            model.Delete(randomInsultId);
            TempData["message"] += model.Message;
            TempData["error"] += model.Error;
            return RedirectToAction("RandomInsults");
        }
    }
}