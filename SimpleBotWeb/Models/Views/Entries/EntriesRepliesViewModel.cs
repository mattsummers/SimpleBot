using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Factories;
using System.Collections.Generic;

namespace SimpleBotWeb.Models.Views.Entries
{
    public class EntriesRepliesViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public IList<Entry> Entries { get; private set; }
        public int MemberId { get; set; }
        public bool IsAdministrator { get; set; }

        public EntriesRepliesViewModel(int memberId, bool isAdministrator)
        {
            Success = true;
            Message = Error = "";
            Entries = new List<Entry>();
            MemberId = memberId;
            IsAdministrator = isAdministrator;
        }

        public void Load()
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var eh = new EntryHelper(dc);
                Entries = eh.GetEntries();
            }
        }
    }
}
