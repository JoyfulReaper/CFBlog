using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
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
