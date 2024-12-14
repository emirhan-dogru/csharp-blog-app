using BlogApp;
using Data.Abstract;
using Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BlogContext>(options => {
options.UseSqlite(builder.Configuration.GetConnectionString("sqlConntection"));
});

builder.Services.AddScoped<IPostRepository , EfPostRepository>();
builder.Services.AddScoped<ITagRepository , EfTagRepository>();
builder.Services.AddScoped<ICommentRepository , EfCommentRepository>();

var app = builder.Build();

app.UseStaticFiles();

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
    name : "default",
    pattern : "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
