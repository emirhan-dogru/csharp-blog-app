using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Concrete.EfCore;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp
{
    public class SeedData
    {
        public static void CreateSeedData(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { TagId = 1, Text = "Backend" , Url = "backend" },
                        new Tag { TagId = 2, Text = "Frontend" , Url = "frontend" },
                        new Tag { TagId = 3, Text = "Fullstack" , Url = "full-stack" },
                        new Tag { TagId = 4, Text = "Python" , Url = "pyhton" }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User {UserId = 1, UserName = "eemirhandogru"},
                        new User {UserId = 2, UserName = "testusername"}
                    );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post {
                            PostId = 1,
                            Title = "Asp.net core",
                            Content = "Asp.net core dersleri",
                            Url = "asp-net-core",
                            IsActive = true,
                            CreatedAt = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "1.jpg",
                            UserId = 1
                        },
                        new Post {
                            PostId = 2,
                            Title = "Pyhton",
                            Content = "Pyhton dersleri",
                            Url = "pyhton",
                            IsActive = true,
                            CreatedAt = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Take(2).ToList(),
                             Image = "2.jpg",
                            UserId = 1
                        },
                         new Post {
                            PostId = 3,
                            Title = "Nodejs",
                            Content = "Nodejs dersleri",
                            Url = "node-js",
                            IsActive = true,
                            CreatedAt = DateTime.Now.AddDays(-2 ),
                            Tags = context.Tags.Take(2).ToList(),
                             Image = "3.jpg",
                            UserId = 2
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}