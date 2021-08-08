using MVCBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.ViewModels;
using MVCBlog.Services;
using MVCBlog.Data;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Enums;
using X.PagedList;

namespace MVCBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _blogEmailSender;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,
            IBlogEmailSender blogEmailSender,
            ApplicationDbContext context)
        {
            _logger = logger;
            _blogEmailSender = blogEmailSender;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 6;

            //var blogs = _context.Blogs.Where(
            //    b => b.Posts.Any(p => p.ReadyStatus == ReadyStatus.ProductionReady))
            //    .Include(b => b.Author)
            //    .OrderByDescending(b => b.Created)
            //    .ToPagedListAsync(pageNumber, pageSize);

            var blogs = _context.Blogs
                .Include(b => b.Author)
                .OrderByDescending(b => b.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(await blogs);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMe model)
        {
            model.Message = $"{model.Message} <hr> Phone: {model.Phone}";
            await _blogEmailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
