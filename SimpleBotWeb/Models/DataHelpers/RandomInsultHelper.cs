using NPoco;
using SimpleBotWeb.Models.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBotWeb.Models.DataHelpers
{
    public class RandomInsultHelper
    {
        private Database dc;

        public RandomInsultHelper(Database dataContext)
        {
            dc = dataContext;
        }

        #region Entries

        public void SaveRandomInsult(RandomInsult entity)
        {
            if (entity != null)
            {
                dc.Save(entity);
            }
        }

        public IList<RandomInsult> GetRandomInsults()
        {
            return dc.Query<RandomInsult>("select * from randominsults order by Insult").ToList();
        }

        public void DeleteRandomInsult(int randomInsultId)
        {
            dc.Execute("delete from randominsults where RandomInsultId=@0", randomInsultId);
        }

        #endregion
    }
}
