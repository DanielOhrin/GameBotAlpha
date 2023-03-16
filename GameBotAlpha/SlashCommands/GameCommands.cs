using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace GameBotAlpha.SlashCommands
{
    public class GameCommands : ApplicationCommandModule
    {
        [SlashCommand("start", "Starts the game!")]
        public static async Task StartCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Started"));
            //await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            //await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Started"));
        }        
        
        [SlashCommand("end", "Ends the game!")]
        public static async Task EndCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Ended"));
            //await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            //await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Started"));
        }
    }
}
