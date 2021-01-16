namespace Kayn.Core.Interfaces
{
    public interface IDiscordConfiguration
    {
        string Token { get; init; }
        string Game { get; init; }
        string Prefix { get; init; }
        string ClientId { get; init; }
    }
}