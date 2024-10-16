using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Models;
using Data.Abstract;
using Data.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        private ITagRepository _tagRepository;
        public PostsController(IPostRepository postRepository, ITagRepository tagRepository)
        {
            this._postRepository = postRepository;
            this._tagRepository = tagRepository;
        }
        public IActionResult Index()
        {
            return View(
                new PostViewsModel
                {
                    Posts = _postRepository.Posts.ToList()
                }
            );
        }

        public async Task<IActionResult> Details(string? url)
        {
            return View(await _postRepository.Posts.FirstOrDefaultAsync(p => p.Url == url));
        }
    }
}