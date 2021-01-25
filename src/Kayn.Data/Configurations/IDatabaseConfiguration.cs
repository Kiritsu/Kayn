using Kayn.Data.Enums;

namespace Kayn.Data.Configurations
{
    public interface IDatabaseConfiguration
    {
        DatabaseProvider Provider { get; }
        string Hostname { get; }
        string Database { get; }
        int Port { get; }
        string Username { get; }
        string Password { get; }
    }
}