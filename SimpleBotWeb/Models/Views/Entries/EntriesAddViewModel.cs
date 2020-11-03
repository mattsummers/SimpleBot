using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.Factories;
using SimpleBotWeb.Models.Helpers;

namespace SimpleBotWeb.Models.Views.Entries
{
    public class EntriesAddViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public EntriesAddViewModel()
        {
            Success = true;
            Message = Error = "";
        }

        public void Add(string phrase, string response, bool startsWith, bool hidden, int memberId, bool allowRepeat)
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
                Error = "The input phrase contained an invalid regular expression. Figure it out on your own.";
                return;
            }

            using (var dc = DatacontextFactory.GetDatabase())
            {
                var eh = new EntryHelper(dc);
                eh.Add(phrase, response, startsWith, hidden, memberId, allowRepeat);
                Message = "Added";
            }
        }
    }
}
