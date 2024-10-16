using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Abstract;
using Entity;

namespace Data.Concrete.EfCore
{
    public class EfTagRepository : ITagRepository
    {
        private BlogContext _context;
        public EfTagRepository(BlogContext context)
        {
            this._context = context;
        }

        public IQueryable<Tag> Tags => _context.Tags;

        public void CreatePost(Tag tag)
        {
             _context.Tags.Add(tag);
            _context.SaveChanges();
        }
    }
}