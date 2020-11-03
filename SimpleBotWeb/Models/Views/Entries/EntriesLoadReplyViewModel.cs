using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Factories;

namespace SimpleBotWeb.Models.Views.Entries
{
    public class EntriesLoadReplyViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public Entry Entry { get; set; }

        public EntriesLoadReplyViewModel()
        {
            Success = true;
            Message = Error = "";
        }

        public void Load(int entryId)
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var eh = new EntryHelper(dc);
                Entry = eh.GetEntryById(entryId);
            }
        }
    }
}
