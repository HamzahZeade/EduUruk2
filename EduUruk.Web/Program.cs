using EduUruk.DAL.EnitityDAL;
using EduUruk.DAL.Repositories;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;
using FFMpegCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
#region Db Con ..........
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});
#endregion Db Con ..........

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = long.MaxValue; // 100 MB in bytes
    options.MaxRequestBodyBufferSize = int.MaxValue;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
});
//builder.Services.AddIdentity<User, Role>()
//         .AddEntityFrameworkStores<ApplicationDbContext>()
//         .AddDefaultTokenProviders();
// Add services to the container.
//Video Processing Setup

GlobalFFOptions.Configure(new FFOptions { BinaryFolder = "./ffmpeg/bin", TemporaryFilesFolder = "/tmp" });
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var appSett = builder.Configuration.GetSection("AppSettings");
#region Auth

builder.Services.AddScoped<UserRoleRepo>();
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<RoleRepo>();
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<RoleManager<Role>>();
builder.Services.AddScoped<VideoLibraryRepo>();
#endregion
//builder.Services.Configure<FormOptions>(options =>
//{
//    //options.MultipartBodyLengthLimit = 104857600; // 100 MB
//    options.MaxRequestBodySize = 104857600;
//});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.AccessDeniedPath = new PathString("/Account/Login");
//    options.Cookie.Name = "Cookie";
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
//    options.LoginPath = new PathString("/Account/Login");
//    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
//    options.SlidingExpiration = true;
//});
//var cookiePolicyOptions = new CookiePolicyOptions
//{
//    MinimumSameSitePolicy = SameSiteMode.Strict,
//};
// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = "Cookie"; // Specify a unique cookie name
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Adjust expiration time as needed
        options.LoginPath = "/Account/Login"; // Specify the login page path
        options.AccessDeniedPath = "/Account/AccessDenied"; // Specify the access denied page path
                                                            // Add other cookie options as needed
    });
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});
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
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
