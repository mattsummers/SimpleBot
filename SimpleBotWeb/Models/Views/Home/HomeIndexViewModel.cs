using System.Collections.Generic;
using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Factories;

namespace SimpleBotWeb.Models.Views.Home
{
    public class HomeIndexViewModel
    {
        public void Load()
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var neh = new NewsEntryHelper(dc);
                NewsEntries = neh.GetNewsEntries();
            }
        }

        public List<NewsEntry> NewsEntries { get; set; }
    }
}
