using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace GameBotAlpha.AutocompleteProviders
{
    public class UpgradeAutoCompleteProvider : IAutocompleteProvider
    {
        public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            return new List<DiscordAutoCompleteChoice>
            {
                new DiscordAutoCompleteChoice("backpack", "backpack"),
                new DiscordAutoCompleteChoice("generator", "generator")
            };
        }
    }
}
