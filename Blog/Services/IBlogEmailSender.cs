/*
 * Blog Project
 * An ASP.NET MVC Blog
 * Based on Coder Foundry Blog series
 * 
 * Kyle Givler 2021
 * https://github.com/JoyfulReaper/Blog
 */

using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace MVCBlog.Services
{
    public interface IBlogEmailSender : IEmailSender
    {
        Task SendContactEmailAsync(string emailfrom, string name, string subject, string htmlMessage);
    }
}
