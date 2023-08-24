using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Radzen;
using MovieStoreApp.Areas.Identity;
using MovieStoreApp.Data;
using MovieStoreApp.Models;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using MovieStoreApp.Repositories;
using Blazored.SessionStorage;
using MovieStoreApp.ModelsVM;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHxServices();
builder.Services.AddScoped<DialogService>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MoviesAppDBConn");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<MoviesAppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<MovieStoreAppRepositoryEF>();//added
builder.Services.AddBlazoredSessionStorage(); //added
builder.Services.AddSingleton<UserService>(); //added
builder.Services.AddHttpClient();
builder.Services.AddScoped<MovieService>();

builder.Services.AddScoped<HttpServices>();
builder.Services.AddScoped<MovieService>();


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
