using LearnStore.Infrastructure;
using LearnStore.Infrastructure.DataAccess.MsSql;
using LearnStore.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.IdentityModel.Tokens;
using LearnStore.Application.Extensions;
using LearnStore.Infrastructure.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplication();
if (builder.Environment.IsDevelopment())
    builder.Services.UseWindowsUserSecrets<Program>(builder.Configuration);
else
    builder.Services.UseDockerSecrets();
builder.Services.AddMsSqlDataAccess(builder.Configuration);
builder.Services.AddIdentity(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

if(!app.Environment.IsDevelopment())
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