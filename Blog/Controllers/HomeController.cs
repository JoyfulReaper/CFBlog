/*
 * Blog Project
 * An ASP.NET MVC Blog
 * Based on Coder Foundry Blog series
 * 
 * Kyle Givler 2021
 * https://github.com/JoyfulReaper/Blog
 */

using MVCBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.ViewModels;
using MVCBlog.Services;
using MVCBlog.Data;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MVCBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _blogEmailSender;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger,
            IBlogEmailSender blogEmailSender,
            ApplicationDbContext context,
            IImageService imageService,
            IConfiguration configuration)
        {
            _logger = logger;
            _blogEmailSender = blogEmailSender;
            _context = context;
            _imageService = imageService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 6;

            var blogs = _context.Blogs
                .Include(b => b.Author)
                .OrderByDescending(b => b.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            ViewData["HeaderImage"] = $"/img/{_configuration["DefaultHeaderImage"]}";
            ViewData["MainText"] = "Blog";
            ViewData["SubText"] = "A Blog by Kyle Givler";

            return View(await blogs);
        }

        public IActionResult About()
        {
            ViewData["HeaderImage"] = $"/img/{_configuration["DefaultHeaderImage"]}";
            ViewData["MainText"] = "Blog";
            ViewData["SubText"] = "About";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["HeaderImage"] = $"/img/{_configuration["DefaultHeaderImage"]}";
            ViewData["MainText"] = "Blog";
            ViewData["SubText"] = "Contact";

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
