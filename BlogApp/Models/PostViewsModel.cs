using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity;

namespace BlogApp.Models
{
    public class PostViewsModel
    {
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}