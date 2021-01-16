using Kayn.Core.Interfaces;

namespace Kayn.Configurations
{
    public class DiscordConfiguration : IDiscordConfiguration
    {
        public string Token { get; init; }
        public string Game { get; init; }
        public string Prefix { get; init; } = "!";
        public string ClientId { get; init; }
    }
}