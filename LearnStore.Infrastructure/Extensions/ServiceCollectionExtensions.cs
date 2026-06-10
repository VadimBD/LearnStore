using LearnStore.Application.Interfaces;
using LearnStore.Infrastructure.Auth;
using LearnStore.Infrastructure.DataAccess.MsSql;
using LearnStore.Infrastructure.Identity;
using LearnStore.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LearnStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMsSqlDataAccess(this IServiceCollection services, IConfiguration configuration) 
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            var connectionString = configuration.GetConnectionString("LearnStoreApp") ?? throw new InvalidOperationException("Conection string 'LearnStoreMigration' not found.");
            
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
           
            services.AddTransient<ICustomerRepository,EFCustomerRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddTransient<ISellerRepository, EFSellerRepository>();
            services.AddTransient<IAuthorRepository, EFAuthorRepository>();
            services.TryAddSingleton<IPasswordProvider, PasswordProvider>();

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
            var connectionString = configuration.GetConnectionString("LearnStoreIdentity") ?? throw new InvalidOperationException("Identity connection string not found.");
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
            services.AddDbContext<AppIdentityDbContext>((sp, options) =>
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                if (!builder.IntegratedSecurity && string.IsNullOrEmpty(builder.Password))
                {
                    var passwordProvider = sp.GetRequiredService<IPasswordProvider>();
                    var password = passwordProvider.GetPassword(GetConfigOrDefault(configuration, "IdentityUser:PasswordKey", "identity_password"));
                    builder.Password = !string.IsNullOrEmpty(password) ? password : throw new InvalidOperationException("Password for database connection is not provided.");
                }
                options.UseSqlServer(builder.ConnectionString);
            });

            services.Configure<DatabaseInitializerOptions<AppIdentityDbContext>>(opt =>
            {
                opt.ConnectionString = connectionString;
                opt.MigrationUserName = GetConfigOrDefault(configuration, "IdentityMigrationUser:UserName", "learnStore_migrator");
                opt.MigrationPasswordKey = GetConfigOrDefault(configuration, "IdentityMigrationUser:PasswordKey", "migrator_password");
                opt.AppUserName = GetConfigOrDefault(configuration, "IdentityUser:UserName", "learnStoreIdentity");
                opt.AppPasswordKey = GetConfigOrDefault(configuration, "IdentityUser:PasswordKey", "identity_password");
            });

            services.AddScoped<IAuthService, IdentityAuthService>();
            services.AddTransient<IDatabaseInitializer<AppIdentityDbContext>, DatabaseInitializer<AppIdentityDbContext>>();
            services.AddSingleton<IDesignTimeDbContextFactory<AppIdentityDbContext>, AppIdentityDbContextFactory>();
            services.TryAddSingleton<IPasswordProvider, PasswordProvider>();

        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IJwtOptionsProvider, JwtOptionsProvider>();

            services.Configure<JwtOptions>(opt =>
            {
                var provider = new JwtOptionsProvider(configuration, services.BuildServiceProvider().GetRequiredService<ISecretProvider>());
                var options = provider.GetOptions();
                opt.Issuer = options.Issuer;
                opt.Audience = options.Audience;
                opt.Key = options.Key;
                opt.ExpireHours = options.ExpireHours;
            });
            services.AddAuthentication(options =>
            { 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var sp= services.BuildServiceProvider();
                var jwtOptions = sp.GetRequiredService<IOptions<JwtOptions>>().Value;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                };
            });
        }

        public static void AddFileStorageServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileStorageService, FileStorageService>();
        }
        public static void UseWindowsUserSecrets<TMarker>(this IServiceCollection services, IConfigurationBuilder configuration) where TMarker : class
        {
            configuration.AddUserSecrets<TMarker>();
            services.AddSingleton<ISecretProvider, WindowsUserSecretsProvider>();
        }
        public static void UseWindowsUserSecrets(this IServiceCollection services)
        {
            services.AddSingleton<ISecretProvider, WindowsUserSecretsProvider>();
        }
        public static void UseDockerSecrets(this IServiceCollection services)
        {
            services.AddSingleton<ISecretProvider, DockerSecretProvider>();
        }
        private static string GetConfigOrDefault(IConfiguration configuration, string path, string defaultValue)
        {
            var raw = configuration[path];
            return string.IsNullOrWhiteSpace(raw) ? defaultValue : raw;
        }
    }
}
