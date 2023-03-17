using DSharpPlus.SlashCommands;

using GameBotAlpha.Services;

namespace GameBotAlpha.Attributes
{
    public class PlayerStartedGame : SlashCheckBaseAttribute
    {
        public Game _game { private get; set; } = null!;
        public override Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            return Task.FromResult(_game.HasStarted(ctx.Member.Id.ToString()));
        }
    }
}
