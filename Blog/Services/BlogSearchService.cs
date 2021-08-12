/*
 * Blog Project
 * An ASP.NET MVC Blog
 * Based on Coder Foundry Blog series
 * 
 * Kyle Givler 2021
 * https://github.com/JoyfulReaper/Blog
 */

using MVCBlog.Data;
using MVCBlog.Enums;
using MVCBlog.Models;
using System.Linq;

namespace MVCBlog.Services
{
    public class BlogSearchService
    {
        private readonly ApplicationDbContext _context;

        public BlogSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Search(string searchTerm)
        {
            var posts = _context.Posts
                .Where(p => p.ReadyStatus == ReadyStatus.ProductionReady).AsQueryable();

            if (searchTerm != null)
            {
                searchTerm = searchTerm.ToLower();

                posts = posts.Where(
                    p => p.Title.ToLower().Contains(searchTerm) ||
                    p.Abstract.ToLower().Contains(searchTerm) ||
                    p.Content.ToLower().Contains(searchTerm) ||
                    p.Comments.Any(c => c.Body.ToLower().Contains(searchTerm) ||
                                        c.ModeratedBody.ToLower().Contains(searchTerm) ||
                                        c.Author.FirstName.ToLower().Contains(searchTerm) ||
                                        c.Author.LastName.ToLower().Contains(searchTerm) ||
                                        c.Author.DisplayName.ToLower().Contains(searchTerm) ||
                                        c.Author.Email.ToLower().Contains(searchTerm)));
            }

            posts = posts.OrderByDescending(p => p.Created);

            return posts;
        }
    }
}
