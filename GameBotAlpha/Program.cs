using DSharpPlus;
using DSharpPlus.SlashCommands;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

using Microsoft.Extensions.Configuration;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands.EventArgs;
using GameBotAlpha.Attributes;
using GameBotAlpha.Services;

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
                    .AddSingleton<Game>()
                    .BuildServiceProvider()
            });
            slashCommands.RegisterCommands(Assembly.GetExecutingAssembly()); //! Register all slash commands from all classes in the assembly
            slashCommands.SlashCommandErrored += SlashCmdErroredHandler;


            await discord.ConnectAsync();
            await Task.Delay(-1); //! Prevents the task -- connection -- from ever ending
        }

        private static async Task SlashCmdErroredHandler(SlashCommandsExtension _, SlashCommandErrorEventArgs e)
        {
            var failedChecks = ((SlashExecutionChecksFailedException)e.Exception).FailedChecks;
            foreach (var failedCheck in failedChecks)
            {
                if (failedCheck is PlayerStartedGame)
                {
                    await e.Context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Type /start to begin playing. If already started, you can make a new account with /reset"));
                }
            }
        }
    }
}