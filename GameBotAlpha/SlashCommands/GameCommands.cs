using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using GameBotAlpha.Attributes;
using GameBotAlpha.Services;

namespace GameBotAlpha.SlashCommands
{
    public class GameCommands : ApplicationCommandModule
    {
        [SlashCommand("start", "Starts the game!"), PlayerStartedGame(false)]
        public static async Task StartCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Started"));
        }        
        
        [SlashCommand("end", "Ends the game!"), PlayerStartedGame(true)]
        public static async Task EndCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Ended"));
        }
    }
}
