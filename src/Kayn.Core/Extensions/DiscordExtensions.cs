using DSharpPlus.Entities;

namespace Kayn.Core.Extensions
{
    public static class DiscordExtensions
    {
        public static string GetFullName(this DiscordUser user)
        {
            return $"{user.Username}#{user.Discriminator}";
        }
    }
}