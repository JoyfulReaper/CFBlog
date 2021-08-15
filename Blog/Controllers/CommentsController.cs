/*
 * Blog Project
 * An ASP.NET MVC Blog
 * Based on Coder Foundry Blog series
 * 
 * Kyle Givler 2021
 * https://github.com/JoyfulReaper/Blog
 */


using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Models;

namespace MVCBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;

        public CommentsController(ApplicationDbContext context,
            UserManager<BlogUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> OriginalIndex()
        {
            var orginalComments = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.Author)
                .ToListAsync();
            return View("Index", orginalComments);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ModeratedIndex()
        {
            var moderatedComments = await _context.Comments.Where(c => c.Moderated != null)
                .Include(c => c.Post)
                .Include(c => c.Author)
                .ToListAsync();
            return View("Index", moderatedComments);
        }


        //GET: Comments
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var allComments = await _context.Comments
                .Include(p => p.Post)
                .Include(p => p.Author)
                .Include(p => p.Moderator)
                .ToListAsync();
            return View(allComments);
        }

        // GET: Comments/Create
        //public IActionResult Create()
        //{
        //    ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
        //    ViewData["ModeratorId"] = new SelectList(_context.Users, "Id", "Id");
        //    ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Abstract");
        //    return View();
        //}

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Body")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.AuthorId = _userManager.GetUserId(User);
                comment.Created = DateTime.Now;

                _context.Add(comment);
                await _context.SaveChangesAsync();

                comment = await _context.Comments
                    .Include(c => c.Post)
                    .Where(c => c.Id == comment.Id)
                    .FirstOrDefaultAsync();

                if(comment == null)
                {
                    return NotFound();
                }    

                return RedirectToAction("Details", "Posts", new { Slug = comment.Post.Slug }, "commentSection");
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", comment.AuthorId);
            ViewData["ModeratorId"] = new SelectList(_context.Users, "Id", "Id", comment.ModeratorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Abstract", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Body")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var commentDb = await _context.Comments.Include(c => c.Post).FirstOrDefaultAsync(c => c.Id == comment.Id);
                try
                {
                    commentDb.Body = comment.Body;
                    commentDb.Updated = DateTime.Now;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { slug = commentDb.Post.Slug }, "commentSection");
            }

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Moderate(int id, [Bind("Id, Body, ModeratedBody,ModerationType")]Comment comment)
        {
            if(id != comment.Id)
            {
                return NotFound();
            }

            var commentDb = await _context.Comments
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.Id == comment.Id);

            if (ModelState.IsValid)
            {
                try
                {
                    commentDb.ModeratedBody = comment.ModeratedBody;
                    commentDb.ModerationType = comment.ModerationType;
                    commentDb.Moderated = DateTime.Now;
                    commentDb.ModeratorId = _userManager.GetUserId(User);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { slug = commentDb.Post.Slug }, "commentSection");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Moderator)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string slug)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Posts", new { slug }, "commentSection");
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
