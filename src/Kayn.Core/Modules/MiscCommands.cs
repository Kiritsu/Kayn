using System.Threading.Tasks;
using Qmmands;

namespace Kayn.Core.Modules
{
    [Name("Miscellaneous")]
    public class MiscCommands : DiscordModule
    {
        [Command("ping")]
        public Task PingAsync()
        {
            return RespondAsync($"Websocket latency: **{Context.Client.Ping}ms**");
        }
    }
}