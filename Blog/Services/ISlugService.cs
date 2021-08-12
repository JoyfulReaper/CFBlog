/*
 * Blog Project
 * An ASP.NET MVC Blog
 * Based on Coder Foundry Blog series
 * 
 * Kyle Givler 2021
 * https://github.com/JoyfulReaper/Blog
 */

namespace MVCBlog.Services
{
    public interface ISlugService
    {
        string UrlFriendly(string title);

        bool IsUnique(string slug);
    }
}
