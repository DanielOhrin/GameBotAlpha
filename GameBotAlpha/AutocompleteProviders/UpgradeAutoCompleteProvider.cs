using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using GameBotAlpha.Data.Enums;

namespace GameBotAlpha.AutocompleteProviders
{
    public class UpgradeAutoCompleteProvider : IAutocompleteProvider
    {
        public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            return new List<DiscordAutoCompleteChoice>
            {
                new DiscordAutoCompleteChoice("backpack", UpgradeTypes.Backpack),
                new DiscordAutoCompleteChoice("generator", UpgradeTypes.Generator)
            };
        }
    }
}
