using Microsoft.EntityFrameworkCore;
using MvcMessageLogger.DataAccess;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register our Entity Framework context as a service and configure it to connect to a PostgreSQL database using the "MvcMovieDb" property in our appsettings.json. This allows us to use dependency injection to access our context from our controller.
//More info can be found here: https://www.npgsql.org/efcore/api/Microsoft.Extensions.DependencyInjection.NpgsqlServiceCollectionExtensions.html
builder.Services.AddDbContext<MvcMessageLoggerContext>(
    options =>
        options
            .UseNpgsql(builder.Configuration["MESSAGELOGGER DBCONNECTIONSTRING"])
            .UseSnakeCaseNamingConvention());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
