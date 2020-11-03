using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPoco;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Values;

namespace SimpleBotWeb.Models.DataHelpers
{
    public class ConfigurationHelper
    {
        private Database dc;

        public ConfigurationHelper(Database dataContext)
        {
            dc = dataContext;
        }

        public Configuration GetConfiguration()
        {
            dynamic configs = dc.Query<dynamic>("select * from config");
            var results = new Configuration();
            foreach (var item in configs)
            {
                results.Add(item.KeyName, item.KeyValue);
            }

            return results;
        }

        public void SaveConfiguration (Configuration config)
        {
            using (var transaction = dc.GetTransaction())
            {
                dc.Execute("Delete from config");
                foreach (var item in config.AllKeys())
                {
                    dc.Execute("insert into config (keyname, keyvalue) values (@0, @1)", item, config[item]);
                }

                transaction.Complete();
            }
            AppConfiguration.SiteConfiguration = config;
        }
    }
}
