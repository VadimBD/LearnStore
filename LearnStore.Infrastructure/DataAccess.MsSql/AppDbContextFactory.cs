
using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;

namespace LearnStore.Infrastructure.DataAccess.MsSql
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {

        private readonly IDatabaseInitializer<AppDbContext>? _initializer;
        private readonly IConfiguration _configuration;
        private readonly IPasswordProvider _passwordProvider;
        public AppDbContextFactory() 
        { 
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddUserSecrets<AppDbContextFactory>(optional: false)
                .AddEnvironmentVariables()
                .Build();
            _passwordProvider=new LocalSecretPasswordProvider(_configuration);
        }
        public AppDbContextFactory(IDatabaseInitializer<AppDbContext> initializer, IConfiguration configuration, IPasswordProvider passwordProvider)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _passwordProvider = passwordProvider ?? throw new ArgumentNullException(nameof(passwordProvider));
        }

        public AppDbContext CreateDbContext(string[] args)
        {
            var connectionString = GetConnectionString(_configuration);
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
        private string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("LearnStoreMigration")
                                    ?? throw new InvalidOperationException("Conection string 'LearnStoreMigration' not found.");
            var builder = new SqlConnectionStringBuilder(connectionString);
            var migrationUserName = configuration["MigrationUser:UserName"];

            if (!string.IsNullOrEmpty(migrationUserName) && migrationUserName == builder.UserID && string.IsNullOrEmpty(builder.Password) && !builder.IntegratedSecurity) 
            {
                _initializer?.EnsureDatabaseAndUser();

                builder.Password = _passwordProvider.GetPassword("migrator_password");
            }

            return builder.ConnectionString;
        }


    }
}
