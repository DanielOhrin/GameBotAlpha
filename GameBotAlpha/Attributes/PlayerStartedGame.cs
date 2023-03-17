using DSharpPlus.SlashCommands;

using GameBotAlpha.Services;

namespace GameBotAlpha.Attributes
{
    public class PlayerStartedGame : SlashCheckBaseAttribute
    {
        private readonly bool _hasStarted;
        private readonly Game _game;
        public PlayerStartedGame(bool hasStarted)
        {
            _game = new Game();
            _hasStarted = hasStarted;
        }
        public override Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            return Task.FromResult(_game.HasStarted(ctx.Member.Id.ToString()) == _hasStarted);
        }
    }
}
