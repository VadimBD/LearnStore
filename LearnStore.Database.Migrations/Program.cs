using LearnStore.Application.Extensions;
using LearnStore.Database.Migrations;
using LearnStore.Infrastructure;
using LearnStore.Infrastructure.Extensions;
using LearnStore.Infrastructure.Identity;
using LearnStore.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
    config.AddEnvironmentVariables();
    if (context.HostingEnvironment.IsDevelopment())
        config.AddUserSecrets<Program>();
}).ConfigureServices((context, services) =>
{
    services.AddApplication();
    services.AddMsSqlDataAccess(context.Configuration);
    services.AddIdentity(context.Configuration);
    services.AddHostedService<MigrationService>();

    if (context.HostingEnvironment.IsDevelopment())
        services.UseWindowsUserSecrets();
    else
        services.UseDockerSecrets();
}).Build();

await host.RunAsync();