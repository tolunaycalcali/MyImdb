using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyImdb.DAL.Context;
using MyImdb.DAL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConStr"));
});

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequiredLength = 4;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<SecurityStampValidatorOptions>(x =>
{
    x.ValidationInterval = TimeSpan.Zero;
});

CookieBuilder cookie = new CookieBuilder()
{
    Name = "MyImdb",
    HttpOnly = false,
};

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie = cookie;
    opt.AccessDeniedPath = new PathString("/NotAuthorize");
    opt.LoginPath = new PathString("/Auth/Login");
    opt.LogoutPath = new PathString("/Auth/Logout");
    opt.ExpireTimeSpan = TimeSpan.FromDays(1);
    opt.SlidingExpiration = true;

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
