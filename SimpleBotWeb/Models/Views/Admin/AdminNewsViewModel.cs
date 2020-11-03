using System.Collections.Generic;
using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Factories;

namespace SimpleBotWeb.Models.Views.Admin
{
    public class AdminNewsViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public List<NewsEntry> NewsEntries { get; set; }

        public NewsEntry News { get; set; }

        public AdminNewsViewModel()
        {
            NewsEntries = new List<NewsEntry>();
            News = new NewsEntry();
            Message = Error = "";
            Success = true;
        }

        public void Load()
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var neh = new NewsEntryHelper(dc);
                NewsEntries = neh.GetNewsEntries();
            }
        }

        public void Delete(int id)
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var neh = new NewsEntryHelper(dc);
                neh.DeleteNewsEntryById(id);
                Message = "Deleted";
            }
        }

        public void Save(NewsEntry entry)
        {
            if (entry == null)
            {
                Success = false;
                Error = "Error saving entry";
                return;
            }

            using (var dc = DatacontextFactory.GetDatabase())
            {
                var neh = new NewsEntryHelper(dc);
                neh.SaveNewsEntry(entry);
                Message = "Saved";
                News = entry;
            }
        }

        public void LoadNewsEntry(in int id)
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var neh = new NewsEntryHelper(dc);
                News = neh.GetNewsEntryById(id);

                if (News == null)
                {
                    Success = false;
                    Error = "Error loading entry";
                }
            }
        }
    }
}
