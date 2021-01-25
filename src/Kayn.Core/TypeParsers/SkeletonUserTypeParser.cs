using System;
using System.Linq;
using System.Threading.Tasks;
using Kayn.Core.Entities;
using Qmmands;

namespace Kayn.Core.TypeParsers
{
    public class SkeletonUserTypeParser : KaynTypeParser<SkeletonUser>
    {
        public static readonly SkeletonUserTypeParser Instance = new();
        
        protected override async ValueTask<TypeParserResult<SkeletonUser>> ParseAsync(Parameter parameter, string value, DiscordCommandContext ctx)
        {
            if (!ulong.TryParse(value, out var snowflake))
            {
                var memory = value.AsMemory();
                if (memory.Span[0] != '<' || memory.Span[^1] != '>')
                {
                    return new TypeParserResult<SkeletonUser>("Couldn't resolve any Skeleton User. (Incomplete mention)");
                }

                var mentionCheck = memory.Span[2];
                if (mentionCheck == '!' && !ulong.TryParse(memory.Slice(3, memory.Length - 4).ToString(), out snowflake))
                {
                    return new TypeParserResult<SkeletonUser>("Couldn't resolve any Skeleton User. (Invalid mention)");   
                }

                if (!ulong.TryParse(memory.Slice(2, memory.Length - 3).ToString(), out snowflake))
                {
                    return new TypeParserResult<SkeletonUser>("Couldn't resolve any Skeleton User. (Invalid mention)");
                }
            }

            try
            {
                var user = await ctx.Client.GetUserAsync(snowflake);
                return new TypeParserResult<SkeletonUser>(new SkeletonUser(user));
            }
            catch (Exception)
            {
                return new TypeParserResult<SkeletonUser>("Couldn't resolve any Skeleton User. (Invalid snowflake)");
            }
        }
    }
}