using BlogApp;
using Data.Abstract;
using Data.Concrete.EfCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BlogContext>(options => {
options.UseSqlite(builder.Configuration.GetConnectionString("sqlConntection"));
});

builder.Services.AddScoped<IPostRepository , EfPostRepository>();
builder.Services.AddScoped<ITagRepository , EfTagRepository>();
builder.Services.AddScoped<ICommentRepository , EfCommentRepository>();
builder.Services.AddScoped<IUserRepository , EfUserRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

SeedData.CreateSeedData(app);

app.MapControllerRoute(
    name : "postDetails",
    pattern : "posts/{url}",
    defaults :  new {  controller = "Posts" , action = "Details" }
);

app.MapControllerRoute(
    name : "postByTag",
    pattern : "posts/tag/{tag}",
    defaults :  new {  controller = "Posts" , action = "Index" }
);

app.MapControllerRoute(
    name : "registerpage",
    pattern : "register",
    defaults :  new {  controller = "Auth" , action = "Register" }
);

app.MapControllerRoute(
    name : "loginpage",
    pattern : "login",
    defaults :  new {  controller = "Auth" , action = "Login" }
);

app.MapControllerRoute(
    name : "logoutpage",
    pattern : "logout",
    defaults :  new {  controller = "Auth" , action = "Logout" }
);

app.MapControllerRoute(
    name : "default",
    pattern : "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
