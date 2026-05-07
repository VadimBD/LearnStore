using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure.Identity
{
    internal class AppIdentityDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordProvider _passwordProvider;
        private readonly IDatabaseInitializer<AppIdentityDbContext> _databaseInitializer;
        
        public AppIdentityDbContextFactory()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets<AppIdentityDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();
            _passwordProvider= new PasswordProvider(new WindowsUserSecretsProvider(_configuration));
        }

        public AppIdentityDbContextFactory(IConfiguration configuration, IPasswordProvider passwordProvider, IDatabaseInitializer<AppIdentityDbContext> databaseInitializer)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _passwordProvider = passwordProvider ?? throw new ArgumentNullException(nameof(passwordProvider));
            _databaseInitializer = databaseInitializer ?? throw new ArgumentNullException(nameof(databaseInitializer));
        }
        public AppIdentityDbContext CreateDbContext(string[] args)
        { 
            var connectionString = GetConnectionString(_configuration);
            var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new AppIdentityDbContext(optionsBuilder.Options);
        }

        private string GetConnectionString(IConfiguration configuration) 
        { 
            var connectionString = configuration.GetConnectionString("LearnStoreIdentityMigration") ?? throw new InvalidOperationException("Identity connection string not found.");
            var builder = new SqlConnectionStringBuilder(connectionString);
            var migrationUserName = configuration["IdentityMigrationUser:UserName"];
            if (!builder.IntegratedSecurity && !string.IsNullOrEmpty(migrationUserName) && migrationUserName == builder.UserID && string.IsNullOrEmpty(builder.Password))
            {
                _databaseInitializer?.EnsureDatabaseAndUser();
                builder.Password=_passwordProvider.GetPassword("identitymigrator_password");
            }
            return builder.ConnectionString;
        }
    }
}
