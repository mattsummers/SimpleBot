using Microsoft.Extensions.Configuration;
using SimpleBotWeb.Models.Helpers;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace SimpleBotWeb.Models.Factories
{
    public static class MailFactory
    {
        public static IConfigurationSection Configuration { get; }

        static MailFactory()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build().GetSection("EmailSettings");
        }

        public static string DefaultFromAddress
        {
            get { return Configuration.GetValue<string>("DefaultFrom"); }
        }

        public static SmtpClient GetSmtpMailer()
        {
            //return new SmtpObject {SmtpServer = "127.0.0.1"};
            var mail = new SmtpClient();
            mail.Host = Configuration["Server"];
            mail.EnableSsl = ValidationHelper.ParseBool(Configuration["EnableTLS"]);
            mail.Port = ValidationHelper.ParseInt(Configuration["Port"]);
            mail.UseDefaultCredentials = false;
            mail.Credentials = new NetworkCredential(Configuration["Username"], Configuration["Password"]);
            return mail;
        }
    }
}
