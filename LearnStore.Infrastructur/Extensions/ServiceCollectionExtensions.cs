using LearnStore.Infrastructure.DataAccess.MsSql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace LearnStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMsSqlDataAccess(this IServiceCollection services,string connectionString) 
        {
            services.AddDbContext<AppDbContext>((sp,options) =>
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                if (!builder.IntegratedSecurity && string.IsNullOrEmpty(builder.Password)) 
                { 
                    var passwordProvider = sp.GetRequiredService<IPasswordProvider>();
                    var password = passwordProvider.GetPassword("app_password");
                    builder.Password = !string.IsNullOrEmpty(password)? password :throw new InvalidOperationException("Cannot retrieve database password.");
                }
                options.UseSqlServer(builder.ConnectionString);
            });

            services.AddScoped<ICustomerRepository,EFCustomerRepository>();
            services.AddScoped<IOrderRepository, EFOrderRepository>();
            services.AddScoped<IProductRepository, EFProductRepository>();
            services.AddScoped<ISellerRepository, EFSellerRepository>();

            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

            services.AddSingleton<IDesignTimeDbContextFactory<AppDbContext>, AppDbContextFactory>();
        }
     
    }
}
