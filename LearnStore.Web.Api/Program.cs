using LearnStore.Infrastructure;
using LearnStore.Infrastructure.DataAccess.MsSql;
using LearnStore.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using LearnStore.Application.Extensions;
using LearnStore.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
var connectionString = builder.Configuration.GetConnectionString("LearnStoreApp") ?? throw new InvalidOperationException("Connection string 'LearnStoreAppUser' not found.");
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
    builder.Services.AddSingleton<IPasswordProvider, LocalSecretPasswordProvider>();
}
else
{
    builder.Services.AddSingleton<IPasswordProvider, DockerSecretPasswordProvider>();
}
builder.Services.AddMsSqlDataAccess(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (!app.Environment.IsDevelopment())
    ApplyDatabaseMigrations(app);


app.Run();

static void ApplyDatabaseMigrations(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<IDesignTimeDbContextFactory<AppDbContext>>().CreateDbContext([]);
        dbContext.Database.Migrate();
    }
}