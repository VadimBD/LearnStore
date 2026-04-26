using LearnStore.Infrastructure.DataAccess.MsSql;
using LearnStore.Infrastructure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace LearnStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMsSqlDataAccess(this IServiceCollection services, IConfiguration configuration) 
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            var connectionString = configuration.GetConnectionString("LearnStoreMigration") ?? throw new InvalidOperationException("Conection string 'LearnStoreMigration' not found.");
            
            services.AddDbContext<AppDbContext>((sp,options) =>
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                if (!builder.IntegratedSecurity && string.IsNullOrEmpty(builder.Password))
                {
                    var passwordProvider = sp.GetRequiredService<IPasswordProvider>();
                    var password = passwordProvider.GetPassword(GetConfigOrDefault(configuration, "AppUser:PasswordKey", "app_password"));
                    builder.Password = !string.IsNullOrEmpty(password) ? password : throw new InvalidOperationException("Password for database connection is not provided.");
                }
                options.UseSqlServer(builder.ConnectionString);
            });
           
            services.AddScoped<ICustomerRepository,EFCustomerRepository>();
            services.AddScoped<IOrderRepository, EFOrderRepository>();
            services.AddScoped<IProductRepository, EFProductRepository>();
            services.AddScoped<ISellerRepository, EFSellerRepository>();

            services.Configure<DatabaseInitializerOptions<AppDbContext>>(opt =>
            {
                opt.ConnectionString = connectionString;
                opt.MigrationUserName = GetConfigOrDefault(configuration, "MigrationUser:UserName", "learnStore_migrator");
                opt.MigrationPasswordKey = GetConfigOrDefault(configuration, "MigrationUser:PasswordKey", "learnStore_password");
                opt.AppUserName = GetConfigOrDefault(configuration, "AppUser:UserName", "learnStore_app");
                opt.AppPasswordKey = GetConfigOrDefault(configuration, "AppUser:PasswordKey", "app_password");
            });

            services.AddTransient<IDatabaseInitializer<AppDbContext>, DatabaseInitializer<AppDbContext>>();
            services.AddSingleton<IDesignTimeDbContextFactory<AppDbContext>, AppDbContextFactory>();
        }
        public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            var connectionString = configuration.GetConnectionString("LearnStoreIdentityMigration") ?? throw new InvalidOperationException("Identity connection string not found.");
            services.AddDbContext<AppIdentityDbContext>((sp, options) =>
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                if (!builder.IntegratedSecurity && string.IsNullOrEmpty(builder.Password))
                {
                    var passwordProvider = sp.GetRequiredService<IPasswordProvider>();
                    var password = passwordProvider.GetPassword(GetConfigOrDefault(configuration, "AppUser:PasswordKey", "app_password"));
                    builder.Password = !string.IsNullOrEmpty(password) ? password : throw new InvalidOperationException("Password for database connection is not provided.");
                }
                options.UseSqlServer(builder.ConnectionString);
            });

            services.Configure<DatabaseInitializerOptions<AppIdentityDbContext>>(opt =>
            {
                opt.ConnectionString = connectionString;
                opt.MigrationUserName = GetConfigOrDefault(configuration, "IdentityMigrationUser:UserName", "learnStore_migrator");
                opt.MigrationPasswordKey = GetConfigOrDefault(configuration, "IdentityMigrationUser:PasswordKey", "migrator_password");
                opt.AppUserName = GetConfigOrDefault(configuration, "IdentityUser:UserName", "identitymigrator_password");
                opt.AppPasswordKey = GetConfigOrDefault(configuration, "IdentityUser:PasswordKey", "identity_password");
            });

            services.AddTransient<IDatabaseInitializer<AppIdentityDbContext>, DatabaseInitializer<AppIdentityDbContext>>();
            services.AddSingleton<IDesignTimeDbContextFactory<AppIdentityDbContext>, AppIdentityDbContextFactory>();

        }
        private static string GetConfigOrDefault(IConfiguration configuration, string path, string defaultValue)
        {
            var raw = configuration[path];
            return string.IsNullOrWhiteSpace(raw) ? defaultValue : raw;
        }
    }
}
