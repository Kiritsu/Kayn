using System;
using System.Data.Common;
using Kayn.Data.Configurations;
using Kayn.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Kayn.Data
{
    public class KaynDbContext : DbContext
    {
        private readonly IDatabaseConfiguration _configuration;
        private readonly DbConnectionStringBuilder _connectionStringBuilder;

        public KaynDbContext(IDatabaseConfiguration configuration, DbConnectionStringBuilder connectionStringBuilder)
        {
            _configuration = configuration;
            _connectionStringBuilder = connectionStringBuilder;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            
            switch (_configuration.Provider)
            {
                case DatabaseProvider.Postgresql:
                    optionsBuilder.UseNpgsql(_connectionStringBuilder.ConnectionString);
                    break;
                case DatabaseProvider.Sqlite:
                    optionsBuilder.UseSqlite(_connectionStringBuilder.ConnectionString);
                    break;
                case DatabaseProvider.SqlServer:
                    optionsBuilder.UseSqlServer(_connectionStringBuilder.ConnectionString);
                    break;
                default:
                    throw new NotSupportedException("The given database provider is not supported.");
            }
        }
    }
}