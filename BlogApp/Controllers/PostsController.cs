using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Models;
using Data.Abstract;
using Data.Concrete.EfCore;
using Entity;
using Microsoft.AspNetCore.Authorization;
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
            var posts = _postRepository.Posts.Where(i => i.IsActive);

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
              .Include(x => x.User)
              .Include(x => x.Tags)
              .Include(x => x.Comments)
              .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(p => p.Url == url));
        }

        [HttpPost]
        public async Task<JsonResult> AddComment(int id, string Text)
        {
            var post = await _postRepository.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            if (post != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var UserName = User.FindFirstValue(ClaimTypes.Name);
                var Image = User.FindFirstValue(ClaimTypes.UserData);
                var entity = new Comment
                {
                    Text = Text,
                    CreatedAt = DateTime.Now,
                    PostId = id,
                    UserId = int.Parse(userId ?? "")
                };
                _commentRepository.CreateComment(entity);
                //return Redirect("/posts/" + post!.Url); bu işlem ajax işlemi ile yapılıyor

                return Json(new
                {
                    UserName,
                    Text,
                    entity.CreatedAt,
                    Image
                });
            }
            // return RedirectToAction("Index");
            return Json(new { });
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _postRepository.CreatePost(
                    new Post
                    {
                        Title = model.Title,
                        Content = model.Content,
                        Url = model.Url,
                        UserId = int.Parse(userId ?? ""),
                        CreatedAt = DateTime.Now,
                        Image = "1.jpg",
                        IsActive = false
                    }
                );
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRepository.Posts;
            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(x => x.UserId == userId);
            }
            return View(await posts.ToListAsync());
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.Posts.Include(x => x.Tags).FirstOrDefault(x => x.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();

            return View(new PostCreateViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, PostCreateViewModel model, int[] tagIds)
        {
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Post
                {
                    PostId = id,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                };

                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityToUpdate.IsActive = model.IsActive;
                }

                _postRepository.EditPost(entityToUpdate, tagIds);
                return RedirectToAction("List");
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();
            return View(model);
        }
    }
}