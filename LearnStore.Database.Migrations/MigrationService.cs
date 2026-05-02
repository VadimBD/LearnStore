using LearnStore.Infrastructure.DataAccess.MsSql;
using LearnStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LearnStore.Database.Migrations
{
    public class MigrationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostEnvironment _environment;

        public MigrationService(IServiceProvider serviceProvider, IHostEnvironment environment)
        {
            _serviceProvider = serviceProvider;
            _environment = environment;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IDesignTimeDbContextFactory<AppDbContext>>().CreateDbContext([]);
            var identityDb = scope.ServiceProvider.GetRequiredService<IDesignTimeDbContextFactory<AppIdentityDbContext>>().CreateDbContext([]);
            if (!_environment.IsDevelopment())
            {
                Console.WriteLine("Applying migrations...");
                await db.Database.MigrateAsync(cancellationToken);
                await identityDb.Database.MigrateAsync(cancellationToken);
                Console.WriteLine("Migrations applied successfully.");
            }
            else 
            {
                Console.WriteLine("Development mode — migrations are not applied.");
            }
        }

        public  Task StopAsync(CancellationToken cancellationToken)=>Task.CompletedTask;

    }
}
