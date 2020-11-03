using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.Factories;

namespace SimpleBotWeb.Controllers
{
    public class TinyUrlController : BaseController
    {
        public IActionResult Index(string id)
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var tu = new TinyUrlHelper(dc);
                var url = tu.GetTinyUrlByShortUrl(id);
                if (url == null)
                {
                    return NotFound("wtf");
                }

                return Redirect(url.Url);
            }
        }

        [HttpGet]
        public IActionResult Convert(int id)
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var tu = new TinyUrlHelper(dc);
                var eh = new EntryHelper(dc);
                var entry = eh.GetEntryById(id);
                entry.Response = tu.ShortenAllUrlsInString(entry.Response);
                eh.SaveEntry(entry);
                return Content(entry.Response);
            }
        }

        [HttpGet]
        public IActionResult ConvertAll()
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var tu = new TinyUrlHelper(dc);
                var eh = new EntryHelper(dc);
                var entries = eh.GetEntries();
                foreach (var entry in entries)
                {
                    var original = entry.Response;
                    entry.Response = tu.ShortenAllUrlsInString(entry.Response);
                    if (entry.Response != original)
                    {
                        eh.SaveEntry(entry);
                        HttpContext.Response.WriteAsync(entry.EntryId + "<br>\r\n");
                    }
                }

                return Content("Done");
            }
        }
    }
}