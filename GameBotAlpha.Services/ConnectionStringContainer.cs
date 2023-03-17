using Microsoft.Extensions.Configuration;

namespace GameBotAlpha.Services
{
    public class ConnectionStringContainer
    {
        private static string _userSecretsId = "dd0d1a70-6df4-4239-88b6-52fda3d47214";
        protected static string _connectionString
        {
            get
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets(_userSecretsId);
                IConfigurationRoot app = builder.Build();

                return app["DefaultConnectionString"];
            }
        }
    }
}
