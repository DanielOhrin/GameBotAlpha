using Azure;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using GameBotAlpha.Attributes;
using GameBotAlpha.AutocompleteProviders;
using GameBotAlpha.Data.Enums;
using GameBotAlpha.Data.Models;
using GameBotAlpha.Services;

namespace GameBotAlpha.SlashCommands
{
    public class GameCommands : ApplicationCommandModule
    {
        public Game _game { private get; set; }

        [SlashCommand("start", "Starts the game!"), PlayerStartedGame(false)]
        public async Task StartCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            _game.Start(ctx.Member.Id.ToString());

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Started the game."));
        }        
        
        [SlashCommand("reset", "Resets your profile"), PlayerStartedGame(true)]
        public async Task EndCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            _game.Reset(ctx.Member.Id.ToString());

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Your account has been reset. If this was a mistake, please contact xQgamers#0179"));
        }

        [SlashCommand("upgrade", "Upgrade your items!"), PlayerStartedGame(true)]
        public async Task UpgradeCommand(InteractionContext ctx, [Option("item", "The thing you want to upgrade", true)][Autocomplete(typeof(UpgradeAutoCompleteProvider))] UpgradeTypes upgradeType)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            
            string response;
            KeyValuePair<bool, int> result = _game.Upgrade(ctx.Member.Id.ToString(), upgradeType);

            if (result.Value == 0)
            {
                response = $"You cannot upgrade your {Enum.GetName(typeof(UpgradeTypes), upgradeType)} any further.";
            }
            else
            {
                if (result.Key == false)
                {
                    response = $"Insufficient funds. Next {Enum.GetName(typeof(UpgradeTypes), upgradeType)} upgrade costs {result.Value}";
                }
                else
                {
                    response = $"Success! You upgraded your {Enum.GetName(typeof(UpgradeTypes), upgradeType)} for {result.Value}";
                }
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(response));
        }

        [SlashCommand("backpack", "Shows the contents of your backpack"), PlayerStartedGame(true)]
        public async Task BackpackCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            KeyValuePair<int, Backpack> result = _game.Backpack(ctx.Member.Id.ToString());

            string response = $"Backpack: {result.Key}/{result.Value.ItemLimit}";

            if (result.Key == result.Value.ItemLimit)
            {
                response += "\n\nYour backpack is full!";
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(response));
        }

        [SlashCommand("sell", "sells the contents of your backpack"), PlayerStartedGame(true)]
        public async Task SellCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            Dictionary<string, object> result = _game.Sell(ctx.Member.Id.ToString());

            /*! Result structure:
                { "StartingBalance", profile.Balance },
                { "ItemName", profile.Generator.Item.Name },
                { "ItemAmount", 0 },
                { "Profit", 0 },
                { "NewBalance", 0 }
             */
            
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Sold {result["ItemAmount"]} {result["ItemName"]} for {result["Profit"]}. New balance: {result["NewBalance"]}"));
        }

        [SlashCommand("profile", "Displays general information about your current profile"), PlayerStartedGame(true)]
        public async Task ProfileCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            UserProfile profile = _game.GetProfile(ctx.Member.Id.ToString());

            string response = $"Start Date: {profile.DateCreated}\n Generator: {profile.Generator.Name}\n Backpack: {profile.Backpack.Name}\n Balance: {profile.Balance}";

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(response));
        }
    }
}
