using LearnStore.Infrastructure.DataAccess.MsSql;
using Microsoft.Extensions.DependencyInjection;

namespace LearnStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMsSqlDataAccess(this IServiceCollection services,string connectionString) 
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<ICustomerRepository,EFCustomerRepository>();
            services.AddScoped<IOrderRepository, EFOrderRepository>();
            services.AddScoped<IProductRepository, EFProductRepository>();
            services.AddScoped<ISellerRepository, EFSellerRepository>();

            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddSingleton<IPasswordProvider, DockerSecretPasswordProvider>();

            services.AddSingleton<IDesignTimeDbContextFactory<AppDbContext>, AppDbContextFactory>();
        }
    }
}
