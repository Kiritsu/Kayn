using System.Threading.Tasks;
using DSharpPlus.Entities;
using Qmmands;

namespace Kayn.Core.TypeParsers
{
    public class DiscordUserTypeParser : KaynTypeParser<DiscordUser>
    {
        public static readonly DiscordUserTypeParser Instance = new();
        
        protected override ValueTask<TypeParserResult<DiscordUser>> ParseAsync(Parameter parameter, string value, DiscordCommandContext ctx)
        {
            throw new System.NotImplementedException();
        }
    }
}