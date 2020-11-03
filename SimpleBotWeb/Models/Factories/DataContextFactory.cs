using NPoco;
using SimpleBotWeb.Models.Values;

namespace SimpleBotWeb.Models.Factories
{
    public class DatacontextFactory
    {
        public static Database GetDatabase()
        {
            return new Database(AppConfiguration.ConnectionString, DatabaseType.MySQL, MySql.Data.MySqlClient.MySqlClientFactory.Instance);
        }
    }
}
