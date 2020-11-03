using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Factories;

namespace SimpleBotWeb.Models.Views.Admin
{
    public class AdminConfigViewModel
    {
        public Configuration Config { get; set; }

        public void Load()
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var ch = new ConfigurationHelper(dc);
                Config = ch.GetConfiguration();
            }
        }

        public void Save(Configuration config)
        {
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var ch = new ConfigurationHelper(dc);
                ch.SaveConfiguration(config);
                Config = config;
            }
        }
    }
}
