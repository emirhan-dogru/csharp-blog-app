using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Models;
using Data.Abstract;
using Data.Concrete.EfCore;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        private ITagRepository _tagRepository;
        private ICommentRepository _commentRepository;
        public PostsController(IPostRepository postRepository, ITagRepository tagRepository, ICommentRepository commentRepository)
        {
            this._postRepository = postRepository;
            this._tagRepository = tagRepository;
            this._commentRepository = commentRepository;
        }
        public async Task<IActionResult> Index(string tag)
        {
            var posts = _postRepository.Posts;

            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));
            }
            return View(
                new PostViewsModel
                {
                    Posts = await posts.ToListAsync(),
                }
            );
        }

        public async Task<IActionResult> Details(string? url)
        {
            return View(await _postRepository
            .Posts
              .Include(x => x.Tags)
              .Include(x => x.Comments)
              .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(p => p.Url == url));
        }

        [HttpPost]
        public async Task<JsonResult> AddComment(int id, string UserName, string Text)
        {
            var post = await _postRepository.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            if (post != null) 
            {
                var entity = new Comment
                {
                    Text = Text,
                    CreatedAt = DateTime.Now,
                    PostId = id,
                    User = new User { UserName = UserName, Image = "p1.jpg" }
                };
                _commentRepository.CreateComment(entity);
                //return Redirect("/posts/" + post!.Url); bu işlem ajax işlemi ile yapılıyor

                return Json(new {
                    UserName,
                    Text,
                    entity.CreatedAt,
                    entity.User.Image
                });
            }
            // return RedirectToAction("Index");
            return Json(new {});
        }
    }
}