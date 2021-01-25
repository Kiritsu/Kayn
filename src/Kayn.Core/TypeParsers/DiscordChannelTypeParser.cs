using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Qmmands;

namespace Kayn.Core.TypeParsers
{
    public class DiscordChannelTypeParser : KaynTypeParser<DiscordChannel>
    {
        public static readonly DiscordChannelTypeParser Instance = new();
        
        protected override ValueTask<TypeParserResult<DiscordChannel>> ParseAsync(Parameter parameter, string value, DiscordCommandContext ctx)
        {
            if (!ulong.TryParse(value, out var snowflake))
            {
                var memory = value.AsMemory();
                if (memory.Span[0] != '<' || memory.Span[^1] != '>')
                {
                    return new TypeParserResult<DiscordChannel>("Couldn't resolve any Channel. (Incomplete mention)");
                }

                var mentionCheck = memory.Span[1];
                if (mentionCheck == '#' && !ulong.TryParse(memory.Slice(2, memory.Length - 3).ToString(), out snowflake))
                {
                    return new TypeParserResult<DiscordChannel>("Couldn't resolve any Channel. (Invalid mention)");
                }
            }

            if (!ctx.Guild.Channels.TryGetValue(snowflake, out var channel))
            {
                return new TypeParserResult<DiscordChannel>("Couldn't resolve any Channel. (Unknown snowflake)");
            }

            return new TypeParserResult<DiscordChannel>(channel);
        }
    }
}