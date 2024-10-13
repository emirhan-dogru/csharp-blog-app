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

var app = builder.Build();

app.UseStaticFiles();

SeedData.CreateSeedData(app);

app.MapDefaultControllerRoute();

app.Run();
