/*
 * Blog Project
 * An ASP.NET MVC Blog
 * Based on Coder Foundry Blog series
 * 
 * Kyle Givler 2021
 * https://github.com/JoyfulReaper/Blog
 */

using System.ComponentModel.DataAnnotations;

namespace MVCBlog.ViewModels
{
    public class ContactMe
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 5)]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 5)]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 12)]
        public string Message { get; set; }
    }
}
