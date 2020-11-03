using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Factories;
using System.Collections.Generic;

namespace SimpleBotWeb.Models.Views.Entries
{
    public class EntriesRandomInsultsViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public IList<RandomInsult> RandomInsults { get; set; }

        public EntriesRandomInsultsViewModel()
        {
            Success = true;
            Message = Error = "";
            RandomInsults = new List<RandomInsult>();
        }

        public void Load()
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var rh = new RandomInsultHelper(dc);
                RandomInsults = rh.GetRandomInsults();
            }
        }

        public void Delete(int randomInsultId)
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var rh = new RandomInsultHelper(dc);
                rh.DeleteRandomInsult(randomInsultId);
                Message = "DELETED... NEXT!";
            }
        }

        public void Add(string randomInsult)
        {
            if (string.IsNullOrWhiteSpace(randomInsult))
            {
                Success = false;
                Error = "You didn't enter anything, idiot.";
                return;
            }

            using (var dc = DatacontextFactory.GetDatabase())
            {
                var rh = new RandomInsultHelper(dc);
                var insult = new RandomInsult
                {
                    Insult = randomInsult
                };
                rh.SaveRandomInsult(insult);
                Message = "YEAH OK, WHATEVER";
            }
        }
    }
}
