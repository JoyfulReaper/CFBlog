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

namespace MVCBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _blogEmailSender;

        public HomeController(ILogger<HomeController> logger,
            IBlogEmailSender blogEmailSender)
        {
            _logger = logger;
            _blogEmailSender = blogEmailSender;
        }

        public IActionResult Index()
        {
            return View();
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
