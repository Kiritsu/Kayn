using System.Data.Common;
using Kayn.Data.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Kayn.Data.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddPostgresqlDatabaseContext(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDatabaseConfiguration, DefaultDatabaseConfiguration>()
                .AddSingleton<DbConnectionStringBuilder>(x =>
                {
                    var config = x.GetRequiredService<IDatabaseConfiguration>();
                    
                    return new NpgsqlConnectionStringBuilder
                    {
                        Host = config.Hostname,
                        Port = config.Port,
                        Database = config.Database,
                        Username = config.Username,
                        Password = config.Password
                    };
                })
                .AddDbContext<KaynDbContext>(ServiceLifetime.Transient);
        }
    }
}