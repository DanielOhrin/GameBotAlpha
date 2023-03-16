using Microsoft.Extensions.Configuration;

namespace GameBotAlpha.Services
{
    public class ConnectionStringContainer
    {
        protected static string _connectionString
        {
            get
            {
                IConfigurationBuilder builder = new ConfigurationBuilder();
                IConfigurationRoot app = builder.Build();

                return app["DefaultConnectionString"];
            }
        }
    }
}
