/*
 * Blog Project
 * An ASP.NET MVC Blog
 * Based on Coder Foundry Blog series
 * 
 * Kyle Givler 2021
 * https://github.com/JoyfulReaper/Blog
 */

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MVCBlog.Services
{
    public interface IImageService
    {
        Task<byte[]> EncodeImageAsync(IFormFile file);
        Task<byte[]> EncodeImageAsync(string fileName);

        string DecodeImage(byte[] data, string type);
        string ContentType(IFormFile file);
        int Size(IFormFile file);
    }
}
