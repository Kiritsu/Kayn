using Kayn.Data.Enums;

namespace Kayn.Data.Configurations
{
    public class DefaultDatabaseConfiguration : IDatabaseConfiguration
    {
        public DatabaseProvider Provider { get; init; }
        public string Hostname { get; init; }
        public string Database { get; init; }
        public int Port { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
    }
}