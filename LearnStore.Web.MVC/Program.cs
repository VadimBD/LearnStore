using LearnStore.Application.Extensions;
using LearnStore.Infrastructure;
using LearnStore.Infrastructure.DataAccess.MsSql;
using LearnStore.Infrastructure.Extensions;
using LearnStore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;


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
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("uk")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture =
        new Microsoft.AspNetCore.Localization.RequestCulture("uk");

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});

builder.Services.AddLocalization();

builder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization();

var app = builder.Build();
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

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