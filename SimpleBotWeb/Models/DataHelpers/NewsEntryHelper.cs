using NPoco;
using SimpleBotWeb.Models.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBotWeb.Models.DataHelpers
{
    public class NewsEntryHelper
    {
        private Database dc;

        public NewsEntryHelper(Database dataContext)
        {
            dc = dataContext;
        }

        public List<NewsEntry> GetNewsEntries()
        {
            return dc.Query<NewsEntry>("Select * From NewsEntries order by DatePostedUtc desc").ToList();
        }

        public NewsEntry GetNewsEntryById(int id)
        {
            return dc.FirstOrDefault<NewsEntry>("Select * from NewsEntries where NewsId=@0", id);
        }

        public void SaveNewsEntry(NewsEntry entity)
        {
            dc.Save(entity);
        }

        public void DeleteNewsEntry(NewsEntry entity)
        {
            dc.Delete<User>(entity);
        }

        public void DeleteNewsEntryById(int id)
        {
            DeleteNewsEntry(GetNewsEntryById(id));
        }
    }
}
