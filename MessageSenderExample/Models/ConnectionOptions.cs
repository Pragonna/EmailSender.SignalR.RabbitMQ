
using System.Configuration;

namespace MessageSenderExample.Models
{
    public class ConnectionOptions
    {
        public string Uri { get; set; }
        public string SenderName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public static ConnectionOptions Options
        {
            get
            {
                var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = config.AppSettings.Settings;

                var options = new ConnectionOptions
                {
                    Uri = appSettings["Uri"]?.Value,
                    Email = appSettings["Email"]?.Value,
                    Password = appSettings["Password"]?.Value,
                    SenderName = appSettings["SenderName"]?.Value
                };

                return options;
            }
        }
        private ConnectionOptions()
        {
        }
    }
}
