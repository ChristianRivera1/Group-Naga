using Microsoft.EntityFrameworkCore;
using MVC_CortesRivera.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register the database context with SQL Server using the connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("MVC_CortesRiveraContext")
        ?? throw new InvalidOperationException("Connection string not found.")));

builder.Services.AddControllersWithViews();

// Configure session with a 30-minute timeout and secure cookie settings
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Allows access to HttpContext outside of controllers (e.g., in services)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Use a generic error page and enable HSTS in non-development environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Set the default route to redirect users to the Login page
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();