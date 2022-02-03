using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.DTOs;
using MVCBlog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<BlogDTO>> GetBlogs()
        {
            var blogs = new List<BlogDTO>();
            var blogModels = _context.Blogs;

            foreach(var b in blogModels)
            {
                blogs.Add(new BlogDTO
                {
                    Id = b.Id,
                    AuthorId = b.AuthorId,
                    Name = b.Name,
                    Description = b.Description,
                    Created = b.Created,
                    Updated = b.Updated,
                });
            }
            return blogs;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if(blog == null)
            {
                return NotFound();
            }

            var dto = new BlogDTO
            {
                Id = blog.Id,
                AuthorId = blog.AuthorId,
                Name = blog.Name,
                Description = blog.Description,
                Created = blog.Created,
                Updated = blog.Updated,
            };

            return Ok(dto);
        }
    }
}
