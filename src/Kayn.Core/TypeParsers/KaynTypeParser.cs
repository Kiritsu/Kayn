using System.Threading.Tasks;
using Qmmands;

namespace Kayn.Core.TypeParsers
{
    public abstract class KaynTypeParser<T> : TypeParser<T>
    {
        protected abstract ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value,
            DiscordCommandContext context);
        
        public override ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value,
            CommandContext context)
        {
            return ParseAsync(parameter, value, (DiscordCommandContext) context);
        }
    }
}