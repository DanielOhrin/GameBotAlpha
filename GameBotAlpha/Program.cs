using DSharpPlus;
using DSharpPlus.SlashCommands;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

using Microsoft.Extensions.Configuration;

namespace GameBotAlpha
{
    internal class Program
    {
        public static async Task Main()
        {
            //! Import User Secrets
            string userSecretsId = "d5b96224-24aa-468d-8439-4a0d303637d2";
            IConfigurationBuilder builder = new ConfigurationBuilder().AddUserSecrets(userSecretsId);
            IConfigurationRoot app = builder.Build();

            //! Establishes the connection
            DiscordClient discord = new(new DiscordConfiguration
            {
                Token = app["ClientToken"],
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                MinimumLogLevel = LogLevel.Debug //! Logs just about everything
            });

            SlashCommandsExtension slashCommands = discord.UseSlashCommands(new SlashCommandsConfiguration
            {
                Services = new ServiceCollection()
                    .AddSingleton<Random>()
                    .BuildServiceProvider()
            });
            slashCommands.RegisterCommands(Assembly.GetExecutingAssembly()); //! Register all slash commands from all classes in the assembly

            await discord.ConnectAsync();
            await Task.Delay(-1); //! Prevents the task -- connection -- from ever ending
        }
    }
}