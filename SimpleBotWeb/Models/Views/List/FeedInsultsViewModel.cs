using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.Factories;
using System.Linq;

namespace SimpleBotWeb.Models.Views.List
{
    public class FeedInsultsViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public string[] RandomInsults { get; set; }

        public void Load()
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var eh = new RandomInsultHelper(dc);
                var entries = eh.GetRandomInsults();
                RandomInsults = entries.Select(x => x.Insult).ToArray();
            }
        }
    }
}
