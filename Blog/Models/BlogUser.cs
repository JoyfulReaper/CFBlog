using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class BlogUser : IdentityUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(75, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName { get => $"{FirstName} {LastName}"; }

        public byte[] ImageData { get; set; }
        public string ContentType { get; set; }


        [StringLength(125, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string GitHubUrl { get; set; }

        [StringLength(125, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string TwitterUrl { get; set; }

        [StringLength(125, ErrorMessage = "The {0} must be at least {2} and at most {1} characters.", MinimumLength = 2)]
        public string PersonalUrl { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
