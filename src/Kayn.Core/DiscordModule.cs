using System.Threading.Tasks;
using DSharpPlus.Entities;
using Qmmands;

namespace Kayn.Core
{
    public class DiscordModule : ModuleBase<DiscordCommandContext>
    {
        protected Task<DiscordMessage> RespondAsync(string message)
        {
            return Context.Channel.SendMessageAsync(new DiscordMessageBuilder()
                .WithContent(message)
                .WithReply(Context.Message.Id));
        }

        protected Task<DiscordMessage> RespondAsync(DiscordMessageBuilder builder)
        {
            return Context.Channel.SendMessageAsync(builder.WithReply(Context.Message.Id));
        }
    }
}