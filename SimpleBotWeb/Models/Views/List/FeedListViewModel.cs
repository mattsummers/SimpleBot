using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.Factories;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBotWeb.Models.Views.List
{
    public class ReplyResult
    {
        public int EntryId { get; set; }
        public string Reply { get; set; }
        public bool AllowRepeat { get; set; }
        public string Owner { get; set; }
        public string LastEdited { get; set; }
    }

    public class FeedListViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public Dictionary<string, ReplyResult[]> Replies { get; set; }

        public void Load()
        {
            Replies = new Dictionary<string, ReplyResult[]>();
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var eh = new EntryHelper(dc);
                var entries = eh.GetEntries();
                var grouped = entries.GroupBy(x => new { Key = x.Phrase, x.StartsWith }, x => x).OrderBy(x => x.Key.Key);
                foreach (var group in grouped)
                {
                    var theKey = (group.Key.StartsWith ? "^" : "") + group.Key.Key;
                    var theValues = group.Select(x => new ReplyResult { EntryId = x.EntryId, Reply = x.Response, AllowRepeat = x.AllowRepeat, LastEdited = x.LastEdited, Owner = x.Owner }).ToArray();
                    Replies.Add(theKey, theValues);
                }
            }
        }
    }
}
