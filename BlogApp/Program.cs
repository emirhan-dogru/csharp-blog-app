using BlogApp;
using Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BlogContext>(options => {
options.UseSqlite(builder.Configuration.GetConnectionString("sqlConntection"));
});

var app = builder.Build();

SeedData.CreateSeedData(app);

app.MapDefaultControllerRoute();

app.Run();
