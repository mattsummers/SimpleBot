using SimpleBotWeb.Models.DataObjects;

namespace SimpleBotWeb.Models.Values
{
    public static class AppConfiguration
    {
        public static string ConnectionString { get; set; }
        public static string EmailServer { get; set; }
        public static string BaseUrl { get; set; }
        public static Configuration SiteConfiguration { get; set; }
    }
}
