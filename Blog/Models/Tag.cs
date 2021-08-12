/*
 * Blog Project
 * An ASP.NET MVC Blog
 * Based on Coder Foundry Blog series
 * 
 * Kyle Givler 2021
 * https://github.com/JoyfulReaper/Blog
 */

using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        [Display(Name = "Tag")]
        public string Text { get; set; }

        // Navigation Properties
        public virtual Post Post { get; set; }
        public virtual BlogUser Author { get; set; }

    }
}
