using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Factories;
using SimpleBotWeb.Models.Helpers;

namespace SimpleBotWeb.Models.Views.Entries
{
    public class EntriesEditViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string Phrase { get; set; }

        public EntriesEditViewModel()
        {
            Success = true;
            Message = Error = "";
        }

        public void Edit(int entryId, int memberId, string phrase, string response, bool startsWith, bool hidden, bool allowRepeat)
        {
            if (string.IsNullOrWhiteSpace(phrase) || string.IsNullOrWhiteSpace(response))
            {
                Success = false;
                Error = "Fill out the fields properly";
                return;
            }

            if (!UtilityHelper.IsValidRegex(phrase))
            {
                Success = false;
                Error = "The input phrase contained an invalid regular expression. Figure it out on your own. I'm not your teacher.";
                return;
            }

            using (var dc = DatacontextFactory.GetDatabase())
            {
                var eh = new EntryHelper(dc);
                //var isOwner = eh.IsEntryOwner(entryId, memberId);
                //if (!isOwner)
                //{
                //    Success = false;
                //    Error = "Nice try, but you aren't the owner of this entry. Kill-droids have been dispatched to your location.";
                //    return;
                //}

                var entry = eh.GetEntryById(entryId);
                if (entry == null)
                {
                    Success = false;
                    Error = "ID # not found. What kind of funny business are you up to?";
                    return;
                }
                entry.Phrase = phrase;
                entry.StartsWith = startsWith;
                entry.Response = response;
                entry.Hidden = hidden;
                entry.AllowRepeat = allowRepeat;
                eh.SaveEntry(entry);
            }
        }

        public void Delete(int entryId)
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var eh = new EntryHelper(dc);
                var entry = eh.GetEntryById(entryId) ?? new Entry();
                Phrase = entry.Phrase;
                eh.Delete(entryId);
                Success = true;
                Message = "Deleted.. NEXT";
            }
        }
    }
}
