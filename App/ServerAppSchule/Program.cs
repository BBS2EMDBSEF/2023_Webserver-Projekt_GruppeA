using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using MySqlConnector;
using ServerAppSchule.Services;
using ServerAppSchule.Areas.Identity;
using ServerAppSchule.Data;
using ServerAppSchule.Services.BackgroundServices;
using ServerAppSchule.Factories;
using Microsoft.Extensions.DependencyInjection;
using ServerAppSchule.Models;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using BlazorDownloadFile;

var builder = WebApplication.CreateBuilder(args);
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<User>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddBlazorDownloadFile();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<User>>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<MigrationService>();
builder.Services.AddHostedService<MigrationBackgroundService>();
builder.Services.AddScoped<MySqlConnection>(_ => new MySqlConnection(connectionString));
builder.Services.AddScoped<ApplicationDbContextFactory>(provider =>
{
    var dbContextOptions = provider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
    return new ApplicationDbContextFactory(dbContextOptions);

});
builder.Services.AddTransient<ApplicationDbContext>();
builder.Services.AddMudServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
